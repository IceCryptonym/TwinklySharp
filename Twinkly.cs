using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwinklySharp
{
    public class Twinkly
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions() {
            Converters = {
                new JsonStringEnumConverter(new LowerCaseNamingPolicy())
            },
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        private readonly string baseUri;
        public IPAddress IP { get; init; }
        public string DeviceId { get; init; }

        public string? AuthToken { get; protected set; }
        public int? ExpiresIn { get; protected set; }

        public Twinkly(IPAddress ip, string deviceId)
        {
            baseUri = "http://" + ip.ToString();
            IP = ip;
            DeviceId = deviceId;
        }

        public async Task<bool> Login()
        {
            string challenge = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            LoginModel loginModel = new LoginModel(challenge);
            LoginResponseModel loginRespModel = await Post<LoginModel, LoginResponseModel>(baseUri + Urls.LOGIN, loginModel);
            if (loginRespModel.StatusCode != 1000)
                return false;

            client.DefaultRequestHeaders.Add("X-Auth-Token", loginRespModel.AuthToken);

            VerifyModel verifyModel = new VerifyModel(loginRespModel.ChallengeResponse);
            StatusCodeModel verifyRespModel = await Post<VerifyModel, StatusCodeModel>(baseUri + Urls.VERIFY, verifyModel);
            if (verifyRespModel.StatusCode != 1000)
                return false;

            AuthToken = loginRespModel.AuthToken;
            ExpiresIn = loginRespModel.ExpiresIn;

            return true;
        }

        public async Task<DeviceDetailsModel> GetDeviceDetails()
        {
            return await Get<DeviceDetailsModel>(baseUri + Urls.GESTALT);
        }

        public async Task<string> GetDeviceName()
        {
            return (await Get<DeviceNameModel>(baseUri + Urls.DEVICE_NAME)).DeviceName;
        }

        public async Task<LedModeResponseModel> GetLedMode()
        {
            return await Get<LedModeResponseModel>(baseUri + Urls.LED_MODE);
        }

        public async Task<int> SetLedMode(LedModeModel model)
        {
            return (await Post<LedModeModel, StatusCodeModel>(baseUri + Urls.LED_MODE, model)).StatusCode;
        }

        public async Task<LedColorResponseModel> GetLedColor()
        {
            return await Get<LedColorResponseModel>(baseUri + Urls.LED_COLOR);
        }

        public async Task<int> SetLedColorRgb(LedColorRgbModel model)
        {
            return (await Post<LedColorRgbModel, StatusCodeModel>(baseUri + Urls.LED_COLOR, model)).StatusCode;
        }

        public async Task<int> SetLedColorHsv(LedColorHsvModel model)
        {
            return (await Post<LedColorHsvModel, StatusCodeModel>(baseUri + Urls.LED_COLOR, model)).StatusCode;
        }

        private async Task<int> CreateMovieEntry(MovieCreateModel model)
        {
            return (await Post<MovieCreateModel, StatusCodeModel>(baseUri + Urls.MOVIES_NEW, model)).StatusCode;
        }

        public async Task<int> UploadMovie(Movie movie, bool enable = false)
        {
            LedMode setToMode = LedMode.Movie;
            if (!enable)
            {
                LedModeResponseModel currentLedMode = await GetLedMode();
                setToMode = currentLedMode.Mode;
            }

            await SetLedMode(new LedModeModel(LedMode.Off));
            
            int createMovieStatusCode = await CreateMovieEntry(movie.Details);
            if (createMovieStatusCode != 1000)
                return createMovieStatusCode;

            ByteArrayContent content = new ByteArrayContent(movie.GetBytes());
            content.Headers.Add("Content-Type", "application/octet-stream");
            HttpResponseMessage response = await client.PostAsync(baseUri + Urls.MOVIES_FULL, content);

            int sendMovieStatusCode = (await response.Content.ReadFromJsonAsync<StatusCodeModel>(options))!.StatusCode;
            if (sendMovieStatusCode != 1000)
                return sendMovieStatusCode;

            await SetLedMode(new LedModeModel(setToMode));

            if (enable)
                return await SetCurrentMovie(movie.Details.Guid);

            return 1000;
        }

        public async Task<int> SetCurrentMovie(Guid guid)
        {
            return (await Post<MovieCurrentModel, StatusCodeModel>(baseUri + Urls.MOVIES_CURRENT, new MovieCurrentModel(guid))).StatusCode;
        }

        public async Task<int> SetMovieConfig(MovieCreateModel model, int ledCount)
        {
            MovieConfigModel config = new MovieConfigModel(model.FrameRate * 1000, ledCount, model.FrameCount);
            return (await Post<MovieConfigModel, StatusCodeModel>(baseUri + Urls.MOVIE_CONFIG, config)).StatusCode;
        }

        private async Task<TResponse> Post<TSend, TResponse>(string uri, TSend model)
        {
            JsonContent content = JsonContent.Create(model, null, options);
            await content.LoadIntoBufferAsync();
            HttpResponseMessage response = await client.PostAsync(uri, content);
            return (await response.Content.ReadFromJsonAsync<TResponse>())!;
        }

        private async Task<TResponse> Get<TResponse>(string uri)
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            return (await response.Content.ReadFromJsonAsync<TResponse>(options))!;
        }
    }
}