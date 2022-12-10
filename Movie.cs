using System.Linq;

namespace TwinklySharp
{
    public class Movie
    {
        public MovieCreateModel Details { get; }

        private readonly List<Frame> frames;
        protected readonly bool hasWhiteLed;

        public Movie(MovieCreateModel movieDetails)
        {
            frames = new List<Frame>();
            hasWhiteLed = movieDetails.ColorType == MovieLedColorType.RGBW_RAW;
            Details = movieDetails;
        }

        public Frame CreateFrame()
        {
            Frame nextFrame = ((Frame)frames.LastOrDefault()?.Clone()! ?? new Frame(Details.LedsPerFrame));

            Details.FrameCount += 1;
            frames.Add(nextFrame);
            return nextFrame;
        }

        public void InsertFrame(int index, Frame frame)
        {
            frames.Insert(index, frame);
        }

        public void RemoveFrame(Frame frame)
        {
            frames.Remove(frame);
        }

        public Frame[] GetFrames()
        {
            return frames.ToArray();
        }

        public byte[] GetBytes()
        {
            int offset = (hasWhiteLed ? 4 : 3);
            byte[] data = new byte[frames.Count * (Details.LedsPerFrame * offset)];

            for (int i = 0; i < frames.Count; i++)
            {
                byte[] frameBytes = frames[i].GetBytes(hasWhiteLed);

                for (int j = 0; j < frameBytes.Length; j++)
                {
                    int index = (i * (Details.LedsPerFrame * offset)) + j;
                    data[index] = frameBytes[j];
                }
            }

            return data;
        }

        public class Frame : ICloneable
        {
            public LedColorRgbModel[] Leds { get; }

            public Frame(LedColorRgbModel[] leds)
            {
                Leds = leds;
            }

            public Frame(int ledCount)
            {
                Leds = new LedColorRgbModel[ledCount];
            }

            public void SetAll(LedColorRgbModel color)
            {
                for (int i = 0; i < Leds.Length; i++)
                    Leds[i] = color;
            }

            public byte[] GetBytes(bool includeWhiteLed)
            {
                byte[] data = new byte[Leds.Length * (includeWhiteLed ? 4 : 3)];
                int offset = (includeWhiteLed ? 1 : 0);

                for (int i = 0; i < Leds.Length; i++)
                {
                    LedColorRgbModel led = Leds[i];
                    int index = i * (includeWhiteLed ? 4 : 3); 

                    if (includeWhiteLed)
                        data[index] = led.White;
                    
                    data[index + offset + 0] = led.Red;
                    data[index + offset + 1] = led.Green;
                    data[index + offset + 2] = led.Blue;
                }

                return data;
            }

            public object Clone()
            {
                return new Frame((LedColorRgbModel[])Leds.Clone());
            }
        }
    }
}
