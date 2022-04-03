using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

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

               
                var msg =  await Context.Channel.SendMessageAsync(embed: embed);
                msg.AddReactionAsync(new Emoji("👍"));
               
        }
    }
}