using Newtonsoft.Json;

namespace CentricxBot.Data
{
    public class BaseConfig
    {
    [JsonProperty("Token")] public string Token { get; set; }
    [JsonProperty("Prefix")] public string Prefix { get; set; }
    [JsonProperty("TwitchClientID")] public string TwitchClientID { get; set; }
    [JsonProperty("TwitchOAuth")] public string TwitchOAuth { get; set; }
    [JsonProperty("LiveAlertChannel")] public string LiveAlertChannel { get; set; }
    [JsonProperty("LiveAlertStreamer")] public string LiveAlertStreamer { get; set; }
    [JsonProperty("LavaLinkIP")] public string LavaLinkIP { get; set; }
    [JsonProperty("LavaLinkPort")] public string LavaLinkPort { get; set; }
    [JsonProperty("LavaLinkPassword")] public string LavaLinkPassword { get; set; }
    [JsonProperty("BotRole")] public string BotRole { get; set; }
    [JsonProperty("JoinToCreateChannel")] public string JoinToCreateChannel { get; set; }
    [JsonProperty("TempVoiceCategory")] public string TempVoiceCategory { get; set; }

        public static BaseConfig GetConfig()
            {
                return JsonConvert.DeserializeObject<BaseConfig>(File.ReadAllText("config.json"));
            }
    }
} 