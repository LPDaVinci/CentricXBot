using Discord;
using Discord.Commands;


namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class DM : ModuleBase<SocketCommandContext>
    {
        [Command("dm")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task DMCommand(IGuildUser user, [Remainder] string text = null)
        
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
            var exampleFooter = new EmbedFooterBuilder()
                .WithText("CentricX © 2022")
                .WithIconUrl("https://static-cdn.jtvnw.net/jtv_user_pictures/24447486-ab3e-4871-8ecd-67c5e5cb69d1-profile_image-70x70.png");
            var embed = new EmbedBuilder{};
            embed.WithFooter(exampleFooter)
            .WithTitle("Neue Support Nachricht")
            .WithDescription($"**CentricX**\n\n{text}")
            .WithColor(Color.Blue);
            
            try
        {
            await user.SendMessageAsync("", false, embed.Build());
        }
        catch (Exception)
        {
            await Context.Channel.SendMessageAsync("DMs closed" );
        }
            
               
        }
        }
    }
}