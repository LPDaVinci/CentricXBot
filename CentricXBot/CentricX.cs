using Discord;
using Discord.Commands;
using Discord.WebSocket;
using CentricXBot.Handler;
using CentricXBot.Services;
using Serilog;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Tracking;
using Microsoft.Extensions.Logging;
using CentricxBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Fergun.Interactive;
using Discord.Interactions;

//Name
namespace CentricXBot
{
    class CentricX
    {
        // setup our fields we assign later
        private DiscordSocketClient _client;
        private InteractionService _icommands;
        private IAudioService _audioService;
        private static string _logLevel;

        static void Main(string[] args = null)
        {
            if (args.Count() != 0)
            {
                _logLevel = args[0];
            }
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/centricx.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

             new CentricX().MainAsync().GetAwaiter().GetResult();
            
        }
        public async Task MainAsync()
        {
            // call ConfigureServices to create the ServiceCollection/Provider for passing around the services
            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                var client = services.GetRequiredService<DiscordSocketClient>();
                var icommands = services.GetRequiredService<InteractionService>();
                var audioService = services.GetRequiredService<IAudioService>();
                _client = client;
                _icommands = icommands;
                _audioService = audioService;
                
                // setup logging and the ready event
                client.Ready += ReadyAsync;

                // Token Stuff

                //Bot Token from config.json
                var token = BaseConfig.GetConfig().Token;

                //StartBot
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                await client.SetStatusAsync(UserStatus.Online);
                await client.SetGameAsync("LPDaVinci auf Twitch", "https://twitch.tv/lpdavinci", ActivityType.Streaming);
                
                //Command Handler Class
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                //Handler for Reactions and Buttons
                services.GetRequiredService<ReactionHandler>();
                services.GetRequiredService<ButtonHandler>();

                //LoggingService
                services.GetRequiredService<LoggingService>();

                //LiveAlertHandler
                services.GetRequiredService<TwitchLiveAlertHandler>();

                //UserJoinHandler
                services.GetRequiredService<GuildJoinHandler>();

                //VoiceStateHandler for Auto-VC
                services.GetRequiredService<VoiceStateHandler>();
                           
                //Lavalink Inactivity Tracker
                services.GetRequiredService<InactivityTrackingService>().BeginTracking();       

                await Task.Delay(Timeout.Infinite);
            }
        }
        private async Task ReadyAsync()
        {
            await _audioService.InitializeAsync();
            await _icommands.RegisterCommandsToGuildAsync(205388858265698304);
            _icommands.AddCommandsGloballyAsync
        }


        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            //Get Config Parts
            var lavalinkip = BaseConfig.GetConfig().LavaLinkIP;
            var lavalinkport = BaseConfig.GetConfig().LavaLinkPort;
            var lavalinkpw = BaseConfig.GetConfig().LavaLinkPassword;
     

            var services = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                    GatewayIntents = 
                        GatewayIntents.GuildMembers| 
                        GatewayIntents.GuildMessages| 
                        GatewayIntents.GuildIntegrations| 
                        GatewayIntents.Guilds|
                        GatewayIntents.GuildBans|
                        GatewayIntents.GuildVoiceStates|
                        GatewayIntents.GuildEmojis| 
                        //GatewayIntents.GuildInvites| 
                        GatewayIntents.GuildMessageReactions|
                        GatewayIntents.GuildMessageTyping|
                        GatewayIntents.GuildWebhooks|
                        GatewayIntents.DirectMessageReactions|
                        GatewayIntents.DirectMessages| 
                        GatewayIntents.DirectMessageTyping,                                 
                    MessageCacheSize = 1000,  
                    AlwaysDownloadUsers = true,
            }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<ReactionHandler>()
                .AddSingleton<ButtonHandler>()
                .AddSingleton<TwitchLiveAlertHandler>()
                .AddSingleton<GuildJoinHandler>()
                .AddSingleton<VoiceStateHandler>()
                .AddSingleton(new InteractiveConfig { DefaultTimeout = TimeSpan.FromMinutes(5) }) // Optional config
                .AddSingleton<InteractiveService>()

                //Lavalink
                 .AddSingleton<IAudioService, LavalinkNode>()	
	             .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                 .AddSingleton(new LavalinkNodeOptions 
                 {
                     RestUri = $"http://{lavalinkip}:{lavalinkport}",
	                 WebSocketUri = $"ws://{lavalinkip}:{lavalinkport}",
                     Password = lavalinkpw
                 }
                 )
                 .AddSingleton<InactivityTrackingOptions>()
                 .AddSingleton<InactivityTrackingService>()

                //Logging
                .AddSingleton<LoggingService>()
                .AddLogging(configure => configure.AddSerilog());

            if (!string.IsNullOrEmpty(_logLevel))
            {
                switch (_logLevel.ToLower())
                {
                    case "info":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                            break;
                        }
                    case "error":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
                            break;
                        }
                    case "debug":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
                            break;
                        }
                    default:
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
                            break;
                        }
                }
            }
            else
            {
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            }

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;

        }
    }
}