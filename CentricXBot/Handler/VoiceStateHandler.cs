using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CentricXBot.Handler
{
    public class VoiceStateHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IConfiguration _config;

        public VoiceStateHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _config = services.GetRequiredService<IConfiguration>();
            // take action when we receive a message (so we can process it, and see if it is a valid command)

            _client.UserVoiceStateUpdated += HandleVoiceState;

        }
        public async Task HandleVoiceState(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
           Console.WriteLine($"VoiceStateUpdate: {user} - {before.VoiceChannel?.Name ?? "null"} -> {after.VoiceChannel?.Name ?? "null"}");
        //needs await
        }


        }
    }
