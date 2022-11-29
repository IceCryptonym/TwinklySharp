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
}