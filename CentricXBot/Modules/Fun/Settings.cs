using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace CentricXBot.Modules.Fun
{
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
    public class ConfigObject
    {
        public List<ConfigJsonData> data { get; set; }
    }   
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
        public class Settings : ModuleBase<SocketCommandContext>
    {
        private readonly IConfiguration config;
        [Command("settings")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task SettingsCommand(string arg, string arg2)
        
        {
            var GuildUser = Context.Guild.GetUser(Context.User.Id);
            if (!GuildUser.GuildPermissions.KickMembers)
    {
            await Context.Message.DeleteAsync();
            await ReplyAsync(":warning: `No permission`");
            return;
    }
    else
    {       
          
            string fileName = "config.json";
            string jsonString = File.ReadAllText(fileName);

            var settingsData = JsonConvert.DeserializeObject<ConfigJsonData>(jsonString);

            switch (arg)
            {
            case "prefix":
                settingsData.prefix = arg2;
                break;
            }

            jsonString = JsonConvert.SerializeObject(settingsData);

            var updatedJsonString = JsonConvert.SerializeObject(settingsData, Formatting.Indented);
             File.WriteAllText("config.json", updatedJsonString);
            

        }
        }
    }
}