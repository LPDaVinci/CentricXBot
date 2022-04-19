using System.Text.Json;

namespace CentricxBot.Functions
{
    class JsonFunctions
    {
    public string token { get; set; }
    public string prefix { get; set; }
    public string clientid { get; set; }
    public string oauth { get; set; }
    public string livealertchannel { get; set; }
    public string livealertstreamer { get; set; }
    public string lavalinkpw { get; set; }
    public string lavalinkip { get; set; }
    public string lavalinkport { get; set; }
    public string botrole { get; set; }
    public string autocreatechannelid { get; set; }
    public string autocreatecategoryid { get; set; }

    public static JsonFunctions GetConfig()
        {
            return JsonSerializer.Deserialize<JsonFunctions>(File.ReadAllText("config.json"));
        }
}
} 