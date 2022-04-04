using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CentricXBot.Services

{
    public class ReactionHandler
    {
        // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public ReactionHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _services = services;

            // take action when we receive a message (so we can process it, and see if it is a valid command)

            _client.ReactionAdded += ReactionAdd;

            _client.ReactionRemoved += ReactionRemove;

        }



        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
            

        public async Task ReactionAdd(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            if (_client.GetUser(reaction.UserId).IsBot) return;
            var msg = await message.GetOrDownloadAsync();

                if (msg.Id.Equals(960529258160214036)){

                
                if (reaction.Emote.Name != "👍") return;
                await (reaction.User.Value as SocketGuildUser).AddRoleAsync(913407637075394590);
                _logger.LogInformation($"Rolle [{(reaction.User.Value as SocketGuildUser).Guild.GetRole(913407637075394590)}] hinzugefügt für [{reaction.User}]");
           }
           

        }

        public async Task ReactionRemove(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            if (_client.GetUser(reaction.UserId).IsBot) return;
            var msg = await message.GetOrDownloadAsync();

                if (msg.Id.Equals(960529258160214036)){

                
                if (reaction.Emote.Name != "👍") return;
                await (reaction.User.Value as SocketGuildUser).RemoveRoleAsync(913407637075394590);
                _logger.LogInformation($"Rolle [{(reaction.User.Value as SocketGuildUser).Guild.GetRole(913407637075394590)}] entfernt für [{reaction.User}]");

           }
           

        }
      
    }
}