using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using CentricXBot.Functions;

namespace CentricXBot.Modules.Fun
{
    public class Count : ModuleBase<SocketCommandContext>
    {
        [Command("count")]
        public async Task CountCommand()
        {

         var memberCount = Context.Guild.MemberCount;

         Context.Channel.SendMessageAsync($"{memberCount}");

        }
    }
    
    

        }

    
