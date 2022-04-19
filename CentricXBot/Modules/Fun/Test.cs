using CentricxBot.Data;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task TestComand()
        
        {
            var embed = new EmbedBuilder()
                {
                    Title = "TestEmbed",
                    Description = "Test",
                                
                }.Build();

                var myReaction = new Emoji("👍");

               
                var msg = await Context.Channel.SendMessageAsync(embed: embed);
                var messages = msg.Id;
   
                //Write On Embed the MessageID in a Config to use the reactionHandler or anything else that needs to store an id
                //var settingsData = BaseConfig.GetConfig();
                
                //settingsData.Prefix = messages.ToString();
                //var updatedJsonString = JsonConvert.SerializeObject(settingsData, Formatting.Indented);
                //File.WriteAllText("config.json", updatedJsonString);
                
                await msg.AddReactionAsync(new Emoji("👍"));
               
        }
    }
}