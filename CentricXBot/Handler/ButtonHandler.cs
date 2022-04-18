using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CentricXBot.Handler
{
    public class ButtonHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public ButtonHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();

            // take action when we receive a message (so we can process it, and see if it is a valid command)

            _client.ButtonExecuted += ButtonExecuteHandler;

        }

        public async Task ButtonExecuteHandler(SocketMessageComponent component)
{
        // We can now check for our custom id
            switch(component.Data.CustomId)
        {
        // Since we set our buttons custom id as 'custom-id', we can check for it like this:
            case "test-1":
            // Lets respond by sending a message saying they clicked the button
                await component.RespondAsync($"{component.User.Mention} has clicked the button!");
        break;
            case "test-2":
            // Lets respond by sending a message saying they clicked the button
                await component.RespondAsync($"{component.User.Mention} has clicked the button 2!");
        break;
    }
}
    }
}
