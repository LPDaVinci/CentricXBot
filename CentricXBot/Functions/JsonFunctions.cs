
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CentricxBot.Functions
{
    public static class JsonFunctions
    {
        public static JObject GetConfig()
            {
                // Get the config file.
                using StreamReader configJson = new StreamReader(Directory.GetCurrentDirectory() + @"/Config.json");
                return (JObject)JsonConvert.DeserializeObject(configJson.ReadToEnd());
            }
        public class ConfigJsonData
            {
                [JsonProperty("token")] public string token { get; set; }
                [JsonProperty("prefix")] public string prefix { get; set; }
                [JsonProperty("clientid")] public string clientid { get; set; }
                [JsonProperty("oauth")] public string oauth { get; set; }
                [JsonProperty("live-alert-streamer")] public string alertstreamer { get; set; }
                [JsonProperty("live-alert-channel")] public string alertchannel { get; set; }
                [JsonProperty("lavalink-pw")] public string lavalinkpw { get; set; }
                [JsonProperty("lavalink-ip")] public string lavalinkip { get; set; }
                [JsonProperty("lavalink-port")] public string lavalinkport { get; set; }
                [JsonProperty("botrole")] public string botrole { get; set; }
                [JsonProperty("autocreatechannelid")] public string autocreatechannelid { get; set; }
                [JsonProperty("autocreatecategoryid")] public string autocreatecategoryid { get; set; }
            }
    }
} 