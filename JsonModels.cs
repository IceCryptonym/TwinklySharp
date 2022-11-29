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

    internal record StaticDeviceNameModel(
        [property: JsonPropertyName("name")]string StaticDeviceName,
        [property: JsonPropertyName("code")]int StatusCode
    );

    public record LedColorRgbModel(
        [property: JsonPropertyName("red")]int Red,
        [property: JsonPropertyName("green")]int Green,
        [property: JsonPropertyName("blue")]int Blue
    );

    public record LedColorHsvModel(
        [property: JsonPropertyName("hue")]int Hue,
        [property: JsonPropertyName("saturation")]int Saturation,
        [property: JsonPropertyName("value")]int Brightness
    );

    public record LedColorResponseModel(
        [property: JsonPropertyName("hue")]int Hue,
        [property: JsonPropertyName("saturation")]int Saturation,
        [property: JsonPropertyName("value")]int Brightness,
        [property: JsonPropertyName("red")]int Red,
        [property: JsonPropertyName("green")]int Green,
        [property: JsonPropertyName("blue")]int Blue,
        [property: JsonPropertyName("code")]int StatusCode
    );

    public enum FirmwareFamily
    {
        D,
        F,
        G,
        M
    }

    public enum LedProfile
    {
        RGB,
        RGBW
    }
}