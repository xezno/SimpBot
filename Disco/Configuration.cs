namespace Disco
{
    public class Configuration
    {
        public string Prefix { get; set; }
        public string BotName { get; set; }
        public string BotImage { get; set; }
        public string BotUrl { get; set; }
        public string BotToken { get; set; }
        public bool VerboseMode { get; set; }
        public string ApiEndpoint { get; set; }
        public int Context { get; set; }
        public string CurrentPersonality { get; set; }
        public decimal Temperature { get; set; }
        public decimal TopP { get; set; }
        public decimal TopK { get; set; }
        public string TenorToken { get; set; }
        public int GifDistanceThreshold { get; set; }
        public bool AutoRespond { get; set; }
        public string WebhookUrl { get; set; }
    }

    public class ConfigBucket : DataStructure<Configuration, ConfigBucket>
    {

        public override bool ReadOnly => true;
        public override string Location => "Content/Config.json";

        public static string prefix { get => Instance.Data.Prefix; }
        public static string botName { get => Instance.Data.BotName; }
        public static string botImage { get => Instance.Data.BotImage; }
        public static string botURL { get => Instance.Data.BotUrl; }
        public static string botToken { get => Instance.Data.BotToken; }
        public static bool verboseMode { get => Instance.Data.VerboseMode; }
        public static string apiEndpoint { get => Instance.Data.ApiEndpoint; }
        public static int context { get => Instance.Data.Context; set => Instance.Data.Context = value; }
        public static string currentPersonality { get => Instance.Data.CurrentPersonality; set => Instance.Data.CurrentPersonality = value; }
        public static decimal temperature { get => Instance.Data.Temperature; set => Instance.Data.Temperature = value; }
        public static decimal topP { get => Instance.Data.TopP; set => Instance.Data.TopP = value; }
        public static decimal topK { get => Instance.Data.TopK; set => Instance.Data.TopK = value; }
        public static string tenorToken { get => Instance.Data.TenorToken; set => Instance.Data.TenorToken = value; }
        public static int gifDistanceThreshold { get => Instance.Data.GifDistanceThreshold; set => Instance.Data.GifDistanceThreshold = value; }
        public static bool autoRespond { get => Instance.Data.AutoRespond; set => Instance.Data.AutoRespond = value; }
        public static string webhookUrl { get => Instance.Data.WebhookUrl; }
    }
}