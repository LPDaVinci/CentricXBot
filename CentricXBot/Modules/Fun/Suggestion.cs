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
    public class Suggestion : ModuleBase<SocketCommandContext>
    {
        [Command("suggest")]
        public async Task SuggestCommand([Remainder] string text = null)
        
        {
             if (text == null)
        {
            await Context.Channel.SendMessageAsync("No argument given");
            return;
        } else {

       
            var embed = new EmbedBuilder()
                {
                    Title = $"{text}",
                    Description = "Press on the 👍 to vote for and Press on the 👎 to vote against the suggestion!",
                                
                }.Build();

          
                var msg =  await Context.Channel.SendMessageAsync(embed: embed);
                
                await msg.AddReactionAsync(new Emoji("👍"));
                await msg.AddReactionAsync(new Emoji("👎"));
                 }
               
        }
    }
}