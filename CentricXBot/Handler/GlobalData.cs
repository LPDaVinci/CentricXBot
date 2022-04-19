using System.Text;
using CentricxBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CentricXBot.Handler
{
    public class GlobalData
    {
        public static string ConfigPath { get; set; } = "test.json";
        public static BaseConfig Config { get; set; }
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

       
       public GlobalData(IServiceProvider services)
        {

           _logger = services.GetRequiredService<ILogger<CommandHandler>>();

        }
        public async Task InitializeAsync()
        {
            var json = string.Empty;

            //Check if Config.json Exists.
            if (!File.Exists(ConfigPath))
            {
                json = JsonConvert.SerializeObject(GenerateNewConfig(), Formatting.Indented);
                File.WriteAllText("test.json", json, new UTF8Encoding(false));
                _logger.LogError("No Config file found. A new one gets generated!");
                await Task.Delay(-1);
            }

            //If Config.json exists, get the values and apply them to the Global Property (Config).
           // json = File.ReadAllText(ConfigPath, new UTF8Encoding(false));
            Config = BaseConfig.GetConfig();
        }

        //If no config is found, this structure is generated as an empty config. 
        private static BaseConfig GenerateNewConfig() => new BaseConfig
        {
            Token =  "<Paste your Token here>" ,
            Prefix = ";",
            TwitchClientID = "<TwitchClientID>",
            TwitchOAuth = "<TwitchOAuthToken>",
            LiveAlertChannel = "<Where to Post Live Alert>",
            LiveAlertStreamer = "<Which Streamer to Check Live Status>",
            LavaLinkIP = "weez-node.cf",
            LavaLinkPort = "2333",
            LavaLinkPassword = "FreeLava",
            BotRole = "207112270860255232",
            JoinToCreateChannel = "965259480281546792",
            TempVoiceCategory = "965240216170401862"
        };
    }
}