using System.Text.Json.Serialization;

namespace TwinklySharp
{
    internal record LoginModel(
        [property: JsonPropertyName("challenge")]string Challenge
    );

    internal record LoginResponseModel(
        [property: JsonPropertyName("authentication_token")]string AuthToken,
        [property: JsonPropertyName("challenge-response")]string ChallengeResponse,
        [property: JsonPropertyName("authentication_token_expires_in")]int ExpiresIn,
        [property: JsonPropertyName("code")]int StatusCode
    );

    internal record VerifyModel(
        [property: JsonPropertyName("challenge-response")]string ChallengeResponse
    );

    internal record StatusCodeModel(
        [property: JsonPropertyName("code")]int StatusCode
    );

    public record DeviceDetailsModel(
        [property: JsonPropertyName("product_name")]        string ProductName,
        [property: JsonPropertyName("hardware_version")]    string Version,
        [property: JsonPropertyName("bytes_per_led")]       int LedByteCount,
        [property: JsonPropertyName("hw_id")]               string HardwareId,
        [property: JsonPropertyName("flash_size")]          int FlashSize,
        [property: JsonPropertyName("led_type")]            int LedType,
        [property: JsonPropertyName("product_code")]        string ProductCode,
        [property: JsonPropertyName("fw_family")]           FirmwareFamily Family,
        [property: JsonPropertyName("device_name")]         string DeviceName,
        [property: JsonPropertyName("uptime")]              long Uptime,
        [property: JsonPropertyName("mac")]                 string Mac,
        [property: JsonPropertyName("uuid")]                Guid Guid,
        [property: JsonPropertyName("max_supported_led")]   int SupportedLedCount,
        [property: JsonPropertyName("number_of_led")]       int LedCount,
        [property: JsonPropertyName("led_profile")]         LedProfile Profile,
        [property: JsonPropertyName("frame_rate")]          float FrameRate,
        [property: JsonPropertyName("measured_frame_rate")] float MeasuredFrameRate,
        [property: JsonPropertyName("movie_capacity")]      int MovieCapacity,
        [property: JsonPropertyName("wire_type")]           int WireType,
        [property: JsonPropertyName("copyright")]           string Copyright
    );

    internal record DeviceNameModel(
        [property: JsonPropertyName("name")]string DeviceName,
        [property: JsonPropertyName("code")]int StatusCode
    );

    public record LedModeModel(
        [property: JsonPropertyName("mode")]LedMode Mode,
        [property: JsonPropertyName("effect_id")]int? EffectId = null
    );

    public record LedModeResponseModel(
        [property: JsonPropertyName("mode")]LedMode Mode,
        [property: JsonPropertyName("shop_mode")]int ShopMode,
        [property: JsonPropertyName("code")]int StatusCode
    );

    public record struct LedColorRgbModel(
        [property: JsonPropertyName("red")]byte Red,
        [property: JsonPropertyName("green")]byte Green,
        [property: JsonPropertyName("blue")]byte Blue,
        [property: JsonPropertyName("white")]byte White = 0
    );

    public record struct LedColorHsvModel(
        [property: JsonPropertyName("hue")]int Hue,
        [property: JsonPropertyName("saturation")]byte Saturation,
        [property: JsonPropertyName("value")]byte Brightness
    );

    public record struct LedColorResponseModel(
        [property: JsonPropertyName("hue")]int Hue,
        [property: JsonPropertyName("saturation")]byte Saturation,
        [property: JsonPropertyName("value")]byte Brightness,
        [property: JsonPropertyName("red")]byte Red,
        [property: JsonPropertyName("green")]byte Green,
        [property: JsonPropertyName("blue")]byte Blue,
        [property: JsonPropertyName("code")]int StatusCode
    );

    public record MovieCreateModel(
        [property: JsonPropertyName("name")]string Name,
        [property: JsonPropertyName("unique_id")]Guid Guid,
        [property: JsonPropertyName("descriptor_type")]MovieLedColorType ColorType,
        [property: JsonPropertyName("leds_per_frame")]int LedsPerFrame,
        [property: JsonPropertyName("fps")]int FrameRate
    )
    {
        [JsonPropertyName("frames_number")]
        public int FrameCount { get; internal set; }

        public static MovieCreateModel Create(string name, int frameRate, DeviceDetailsModel deviceDetails)
        {
            return Create(name, frameRate, deviceDetails.Profile == LedProfile.RGB ? MovieLedColorType.RGB_RAW : MovieLedColorType.RGBW_RAW, deviceDetails.LedCount);
        }

        public static MovieCreateModel Create(string name, int frameRate, MovieLedColorType colorType, int ledCount)
        {
            Guid guid = Guid.NewGuid();
            return new MovieCreateModel(name, guid, colorType, ledCount, frameRate);
        }
    }

    internal record MovieCurrentModel(
        [property: JsonPropertyName("unique_id")]Guid Guid
    );

    internal record MovieConfigModel(
        [property: JsonPropertyName("frame_delay")]int FrameDelay,
        [property: JsonPropertyName("leds_number")]int LedCount,
        [property: JsonPropertyName("frames_number")]int FrameCount
    );

    public enum FirmwareFamily
    {
        D,
        F,
        G,
        M
    }

    public enum LedMode
    {
        [JsonPropertyName("off")] Off,
        [JsonPropertyName("color")] Color,
        [JsonPropertyName("demo")] Demo,
        [JsonPropertyName("effect")] Effect,
        [JsonPropertyName("movie")] Movie,
        [JsonPropertyName("playlist")] Playlist,
        [JsonPropertyName("rt")] RealTime
    }

    public enum LedProfile
    {
        RGB,
        RGBW
    }

    public enum MovieLedColorType
    {
        RGB_RAW,
        RGBW_RAW
    }
}