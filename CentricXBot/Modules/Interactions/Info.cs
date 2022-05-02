using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class Info : InteractionModuleBase<SocketInteractionContext>
{
 [UserCommand("my-usercommand")]
    public async Task MyDemo(IUser user)
    {
        if (user is not SocketGuildUser) return;
        var socketUser = (SocketGuildUser)user;
        if (socketUser.JoinedAt.HasValue)
        {
            var joinedAt = socketUser.JoinedAt;
            await RespondAsync($"User joined {joinedAt}").ConfigureAwait(false);
        }
        else
        {
            await RespondAsync("No join date for this user found.").ConfigureAwait(false);
        }
    }

[MessageCommand("Bookmark")]
public async Task Bookmark(IMessage msg)
{
    await RespondAsync("hi");
}
}

