using Discord;
using Discord.Commands;

namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Invite : ModuleBase<SocketCommandContext>
    {
        [Command("invite")]
       public async Task InviteCreate()
{
    
    var GuildUser = Context.Guild.GetUser(Context.User.Id);

    if (!(GuildUser.VoiceChannel == null))
    {
    var invite = await GuildUser.VoiceChannel.CreateInviteAsync(maxAge: null, maxUses: 25, isTemporary: false, isUnique: false);
    await Context.Channel.SendMessageAsync(invite.Url);
    }
    else {
       await Context.Channel.SendMessageAsync("You need to be in a VC to use this command");
    }



    
   

}
}
}