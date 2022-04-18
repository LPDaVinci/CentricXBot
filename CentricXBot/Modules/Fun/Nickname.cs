using Discord;
using Discord.Commands;


namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Nickname : ModuleBase<SocketCommandContext>
    {
        [Command("nick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task NickCommand(IGuildUser user, [Remainder] string text = null)
        
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
 
            await user.ModifyAsync(x => {
                x.Nickname = $"{text}";
            });
       
            
               
        }
        }
    }
}