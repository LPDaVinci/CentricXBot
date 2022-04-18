using Discord;
using Discord.Commands;


namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class PrivText : ModuleBase<SocketCommandContext>
    {
        [Command("text")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task TextCommand([Remainder] string text = null)
        
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
            
          var myChannel = await Context.Guild.CreateTextChannelAsync("test");
          var permissionOverrides = new OverwritePermissions(viewChannel:PermValue.Deny);
          var permissionOverrides2 = new OverwritePermissions(viewChannel:PermValue.Allow);
            await myChannel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, permissionOverrides);
            await myChannel.AddPermissionOverwriteAsync(Context.User, permissionOverrides2);

       
            
               
        }
        }
    }
}