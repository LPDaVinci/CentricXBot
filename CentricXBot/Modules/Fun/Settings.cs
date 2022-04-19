using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CentricxBot.Data;

namespace CentricXBot.Modules.Fun
{
        
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
        
            var settingsData = BaseConfig.GetConfig();

            switch (arg)
            {
            case "prefix":
                settingsData.Prefix = arg2;
                Context.Channel.SendMessageAsync($"Der Prefix wurde auf {arg2} geändert");
                break;
            case "vc":
                settingsData.JoinToCreateChannel = arg2;
                Context.Channel.SendMessageAsync($"Der Voice Channel wurde auf {arg2} geändert");
                break;
            }

            jsonString = JsonConvert.SerializeObject(settingsData);

             var updatedJsonString = JsonConvert.SerializeObject(settingsData, Formatting.Indented);
              File.WriteAllText("config.json", updatedJsonString);
            

        }
        }
    }
} 