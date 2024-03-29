﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using CentricxBot.Data;
using Discord.Interactions;

namespace CentricXBot.Handler
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly InteractionService _icommands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CommandHandler(DiscordSocketClient client, InteractionService icommands, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _icommands = icommands;
            _commands = commands;
            _services = services;

            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
        }
            public async Task InitializeAsync()
            {
                await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
                await _icommands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            // process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;

            // process the command execution results 
            _client.MessageReceived += MessageReceivedAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
            _icommands.SlashCommandExecuted += SlashCommandExecuted;
            _icommands.ContextCommandExecuted += ContextCommandExecuted;
            _icommands.ComponentCommandExecuted += ComponentCommandExecuted;

            }
            public async Task MessageReceivedAsync(SocketMessage rawMessage)
            {
                var prefix = BaseConfig.GetConfig().Prefix;
                var livealertchannel = BaseConfig.GetConfig().LiveAlertChannel;

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

                if (message.Channel is IPrivateChannel) 
                {
                    if (!(message.HasStringPrefix(prefix, ref argPos))) 
                    {
                        ulong channelID = Convert.ToUInt64(livealertchannel);
                        var channel = _client.GetChannel(channelID) as SocketTextChannel;

                        var exampleFooter = new EmbedFooterBuilder()
                            .WithText("CentricX © 2022")
                            .WithIconUrl("https://static-cdn.jtvnw.net/jtv_user_pictures/24447486-ab3e-4871-8ecd-67c5e5cb69d1-profile_image-70x70.png");
                        var embed = new EmbedBuilder{};
                        embed.WithFooter(exampleFooter)
                            .WithTitle("NEW DM")
                            .WithDescription($"**User:** {message.Author.Mention} \n\n**Content:** {message}")
                            .WithCurrentTimestamp()
                            .WithColor(Color.Blue);
                        await channel.SendMessageAsync("", false, embed.Build());
                    }   else{
                                if ((message.HasStringPrefix(prefix, ref argPos) && message.Content.Contains("help"))) 
                                {
                                    await message.Channel.SendMessageAsync("You only can run the help command in the guild not in dm");
                                }   else
                                    {
                                    await message.Channel.SendMessageAsync("No Commands in DM");
                                    return;
                                    }
                            }  
                } 
                if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasStringPrefix(prefix, ref argPos)))
                {
                    return;
                }
                var context = new SocketCommandContext(_client, message);

                // execute command if one is found that matches
                await _commands.ExecuteAsync(context, argPos, _services);
            }
            public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, Discord.Commands.IResult result)
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
                            case CommandError.ObjectNotFound:
                                await context.Channel.SendMessageAsync("User not found.");
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
                        }           
            }
                    private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }    

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction (SocketInteraction arg)
        {
            try
            {
                // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_client, arg);
                await _icommands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if(arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}

