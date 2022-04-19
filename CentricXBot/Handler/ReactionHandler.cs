using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CentricXBot.Handler
{
    public class ReactionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public ReactionHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();

            _client.ReactionAdded += ReactionAdd;
            _client.ReactionRemoved += ReactionRemove;
        }
            public async Task ReactionAdd(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
            {
                if (_client.GetUser(reaction.UserId).IsBot) return;
                var msg = await message.GetOrDownloadAsync();

                    if (msg.Id.Equals(961724051599011910))
                    {

                    
                    if (reaction.Emote.Name != "👍") return;
                    await ((SocketGuildUser)reaction.User.Value).AddRoleAsync(913407637075394590);
                    _logger.LogInformation($"Rolle [{((SocketGuildUser)reaction.User.Value).Guild.GetRole(913407637075394590)}] hinzugefügt für [{reaction.User}]");
                    }
            }
            public async Task ReactionRemove(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
            {
                if (_client.GetUser(reaction.UserId).IsBot) return;
                var msg = await message.GetOrDownloadAsync();

                    if (msg.Id.Equals(961724051599011910))
                    {
                    if (reaction.Emote.Name != "👍") return;
                    await ((SocketGuildUser)reaction.User.Value).RemoveRoleAsync(913407637075394590);
                    _logger.LogInformation($"Rolle [{((SocketGuildUser)reaction.User.Value).Guild.GetRole(913407637075394590)}] entfernt für [{reaction.User}]");
                    }
            }
    }
}