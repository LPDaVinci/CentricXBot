using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CentricXBot.Handler;
using CentricXBot.Services;
using Serilog;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Tracking;
using Microsoft.Extensions.Logging;

//Name
namespace CentricXBot
{

    class CentricX
    {
        // setup our fields we assign later
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;
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
    

        public CentricX()
        {
            //Create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            // build the configuration and assign to _config          
            _config = _builder.Build();
        }
        public async Task MainAsync()
        {
            // call ConfigureServices to create the ServiceCollection/Provider for passing around the services
            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                var audio = services.GetRequiredService<IAudioService>();

                
                // setup logging and the ready event
                services.GetRequiredService<LoggingService>();

                services.GetRequiredService<TwitchLiveAlertHandler>();

                services.GetRequiredService<GuildJoinHandler>();

                services.GetRequiredService<VoiceStateHandler>();

                // this is where we get the Token value from the configuration file, and start the bot
                await client.LoginAsync(TokenType.Bot, _config["token"]);
                await client.StartAsync();
                await client.SetGameAsync("LPDaVinci auf Twitch", "https://twitch.tv/lpdavinci", ActivityType.Streaming);

                _client.Ready += () => audio.InitializeAsync();

                services.GetRequiredService<InactivityTrackingService>().BeginTracking();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();
                
                services.GetRequiredService<ReactionHandler>();

                   await Task.Delay(-1);
            }
        }


        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            var services = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                    GatewayIntents = 
                        GatewayIntents.GuildMembers | 
                        GatewayIntents.GuildMessages | 
                        GatewayIntents.GuildIntegrations | 
                        GatewayIntents.Guilds |
                        GatewayIntents.GuildBans |
                        GatewayIntents.GuildVoiceStates |
                        GatewayIntents.GuildEmojis | 
                        //GatewayIntents.GuildInvites | 
                        GatewayIntents.GuildMessageReactions |
                        GatewayIntents.GuildMessageTyping |
                        GatewayIntents.GuildWebhooks |
                        GatewayIntents.DirectMessageReactions |
                        GatewayIntents.DirectMessages | 
                        GatewayIntents.DirectMessageTyping,                                 
                    MessageCacheSize = 1000,  
            }))
            
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<ReactionHandler>()
                .AddSingleton<TwitchLiveAlertHandler>()
                .AddSingleton<GuildJoinHandler>()
                .AddSingleton<VoiceStateHandler>()
                //Lavalink
                .AddSingleton<IAudioService, LavalinkNode>()	
	            .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                .AddSingleton(new LavalinkNodeOptions 
                {
                    RestUri = $"http://{_config["lavalink-ip"]}:{_config["lavalink-port"]}",
	                WebSocketUri = $"ws://{_config["lavalink-ip"]}:{_config["lavalink-port"]}",
                    Password = _config["lavalink-pw"]
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