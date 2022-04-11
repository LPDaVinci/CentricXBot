using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CentricXBot.Handler

{
    public class CommandHandler
    {
        // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public CommandHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _client.MessageReceived += MessageReceivedAsync;

        }
        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }
            var argPos = 0;

            char prefix = Char.Parse(_config["prefix"]);

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            // execute command if one is found that matches
            await _commands.ExecuteAsync(context, argPos, _services);
        }
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }

            if (!result.IsSuccess)
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            await context.Channel.SendMessageAsync("Bad argument count.");
                            break;
                        case CommandError.UnknownCommand:
                            break;
                        case CommandError.Exception:
                            // This is what happens instead of the catch block.
                            break;
                        default:
                            await context.Channel.SendMessageAsync($"You 👏👏 Broke 👏👏 It ({result.ErrorReason})");
                            break;

                    } else{
                        _logger.LogInformation($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                        return;
                    }           }
        }
    }

