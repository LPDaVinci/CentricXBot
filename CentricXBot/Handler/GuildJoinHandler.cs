using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using CentricxBot.Functions;
using Newtonsoft.Json.Linq;

namespace CentricXBot.Handler
{
    public class GuildJoinHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public GuildJoinHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            // take action when we receive a message (so we can process it, and see if it is a valid command)

            _client.UserJoined += HandleUserJoinedAsync;
            _client.UserLeft += HandleUserLeftAsync;
        }
        public async Task HandleUserJoinedAsync(SocketGuildUser user)
        {
            var botrole = BaseConfig.GetConfig().botrole;
   
            
            if (user.IsBot)
            {
                await user.AddRoleAsync(Convert.ToUInt64($"{botrole}"));
            }
            var channel = _client.GetChannel(958338678286065706) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name}"); //Welcomes the new user
            _logger.LogInformation($"User [{user.Mention}] ist dem Server beigetreten.");
        }

        public async Task HandleUserLeftAsync(SocketGuild guild, SocketUser user)
        {
            var channel = _client.GetChannel(958338678286065706) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Bye {user.Mention} to {guild.Name}"); //Welcomes the new user
           _logger.LogInformation($"User [{user.Mention}] hat den Server verlassen.");
        }

        }
    }
