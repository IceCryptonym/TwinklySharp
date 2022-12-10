namespace TwinklySharp
{
    public static class Urls
    {
        public const string BASE = "/xled/v1/";

        public const string LOGIN = BASE + "login";
        public const string VERIFY = BASE + "verify";

        public const string GESTALT = BASE + "gestalt";
        public const string DEVICE_NAME = BASE + "device_name";

        public const string LED_COLOR = BASE + "led/color";
        public const string LED_MODE = BASE + "led/mode";

        public const string MOVIES_NEW = BASE + "movies/new";
        public const string MOVIES_FULL = BASE + "movies/full";
        public const string MOVIES_CURRENT = BASE + "movies/current";
        public const string MOVIE_CONFIG = BASE + "led/movie/config";
    }
}