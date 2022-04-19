using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;

namespace CentricXBot.Handler
{
    public class ButtonHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public ButtonHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _client.ButtonExecuted += ButtonExecuteHandler;

        }
        public async Task ButtonExecuteHandler(SocketMessageComponent component)
        {
            // We can now check for our custom id
                switch(component.Data.CustomId)
            {
                case "test-1":              
                    await component.Message.DeleteAsync();
                    await component.RespondAsync($"{component.User.Mention} has clicked the button!");
                    await component.DeleteOriginalResponseAsync();
            break;
                case "test-2":
                    await component.RespondAsync($"{component.User.Mention} has clicked the button 2!");
            break;
            }
        }
    }
}
