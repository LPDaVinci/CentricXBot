using Newtonsoft.Json;

namespace CentricxBot.Functions
{
    class BaseConfig
    {
    [JsonProperty("token")] public string token { get; set; }
    [JsonProperty("prefix")] public string prefix { get; set; }
    [JsonProperty("clientid")] public string clientid { get; set; }
    [JsonProperty("oauth")] public string oauth { get; set; }
    [JsonProperty("live-alert-channel")] public string livealertchannel { get; set; }
    [JsonProperty("live-alert-streamer")] public string livealertstreamer { get; set; }
    [JsonProperty("lavalink-pw")] public string lavalinkpw { get; set; }
    [JsonProperty("lavalink-ip")] public string lavalinkip { get; set; }
    [JsonProperty("lavalink-port")] public string lavalinkport { get; set; }
    [JsonProperty("botrole")] public string botrole { get; set; }
    [JsonProperty("autocreatechannelid")] public string autocreatechannelid { get; set; }
    [JsonProperty("autocreatecategoryid")] public string autocreatecategoryid { get; set; }

    public static BaseConfig GetConfig()
        {
            return JsonConvert.DeserializeObject<BaseConfig>(File.ReadAllText("config.json"));
        }
}
} 