using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Clear : ModuleBase<SocketCommandContext>
    {
        private bool weekcheck = false;
        [Command("clear")]
       public async Task ClearAsync(uint amount, ulong bottomMessageId = 0)
{

            var context = Context;
			var guild = context.Guild;
            var channel = (Context.Channel as ITextChannel);
             
            var utcNow = DateTime.UtcNow.AddMinutes(1); //+1 min

            var messageList = new List<IMessage>();

            	if (bottomMessageId != 0) {
				amount--;

				messageList.Add(await channel.GetMessageAsync(bottomMessageId));
			}

            List<ulong> msgToDel = new List<ulong>();
			messageList.AddRange(
				(await channel.GetMessagesAsync((int)amount + 1).FlattenAsync()).Where(m => {
					if (m == null) {
						return false;
					}

                    if ((utcNow - m.Timestamp.UtcDateTime).TotalDays >= 14) {
                        weekcheck = true;
                        return false;
              
					}

					return true;
				})
			);
            var count = messageList.Count() - 1;
            if (weekcheck) {
                msgToDel.Add((await ReplyAsync($":white_check_mark: I have deleted `{count} {(count > 1 ? "messages" : "message")}`!\n\n" +
                ":negative_squared_cross_mark: Because of Discord limitations I can't delete messages past 2 weeks.\n\n" +
                "If you want to clear a whole channel just right-click the channel then select 'Clone Channel'")).Id); //Send a msg, then add the msg ID to the list.
            } else {
                msgToDel.Add((await ReplyAsync($":white_check_mark: I have deleted `{count} {(count > 1 ? "messages" : "message")}`!")).Id); 
            }       
             
            weekcheck = false;   
            Task.Run(async() => { 
                await channel.DeleteMessagesAsync(messageList); 
                await Task.Delay(5000);
                await channel.DeleteMessagesAsync(msgToDel);        
            
            });

}

    }

}