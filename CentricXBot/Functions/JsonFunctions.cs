
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
    }
} 