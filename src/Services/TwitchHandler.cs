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
    public class TwitchHandler
    {
       
    // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public TwitchHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _services = services;

            // hook into these events with the methods provided below
             _client.Ready += TwitchTest;
        }

        public async Task TwitchTest()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Client-ID", $"{_config["clientid"]}"); 
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{_config["oauth"]}");
            HttpResponseMessage response = await client.GetAsync($"https://api.twitch.tv/helix/streams?user_login=lpdavinci");
            HttpContent responseContent = response.Content;

            string responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"ClientID lautet: [{responseBody}]");
        }
    }
}

