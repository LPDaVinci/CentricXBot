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
    public class Clear : ModuleBase<SocketCommandContext>
    {
        [Command("clear")]
       public async Task ClearAsync(int amount)
{
    if (amount <= 0)
    {
        IUserMessage m = await ReplyAsync("The amount of messages to remove must be positive.");
        await Task.Delay(2000);
        await m.DeleteAsync();
        return;
    }
    var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();

    var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);

    // Get the total amount of messages.
    var count = filteredMessages.Count();

    // Check if there are any messages to delete.
    if (count == 0)
    {
        IUserMessage m = await ReplyAsync("Nothing to delete or Text is older than 14 Days");
        await Task.Delay(2000);
        await m.DeleteAsync();
    }
    else
    {
        await (Context.Channel as ITextChannel).DeleteMessagesAsync(filteredMessages);
        IUserMessage m = await ReplyAsync($"Done. Removed {count} {(count > 1 ? "messages" : "message")}.");
        await Task.Delay(2000);
        await m.DeleteAsync();
    }
        await Context.Message.DeleteAsync();
    

}
}
}