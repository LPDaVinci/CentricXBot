using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
    public class Datum
    {
        [JsonProperty("id")] public string ID { get; set; }
        [JsonProperty("userId")] public string user_id { get; set; }
        [JsonProperty("user_name")] public string user_name { get; set; }
        [JsonProperty("game_id")] public string game_id { get; set; }
        [JsonProperty("game_name")] public string game_name { get; set; }
        [JsonProperty("type")] public string type { get; set; }
        [JsonProperty("title")] public string title { get; set; }
        [JsonProperty("viewer_count")] public int viewer_count { get; set; }
        [JsonProperty("thumbnail_url")] public string thumbnail_url { get; set; }
    }
    public class StreamObject
    {
        public List<Datum> data { get; set; }

    }   

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ProfileTwitch
    {
        [JsonProperty("profile_image_url")] public string profile_image_url { get; set; }
    }

    public class ProfileObject
    {
        public List<ProfileTwitch> data { get; set; }
    }

    public async Task TwitchTest()
    {
        //Create new HttpClient
        var client = new HttpClient();
        //Send client-id and oauth token to Twitch API
        client.DefaultRequestHeaders.Add("Client-ID", $"{_config["clientid"]}"); 
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{_config["oauth"]}");
       //Get Stream Info
        HttpResponseMessage response = await client.GetAsync($"https://api.twitch.tv/helix/streams?user_login={_config["twitchchannel"]}");
        HttpContent responseContent = response.Content;
        string jsonString = await response.Content.ReadAsStringAsync();
        var stream = JsonConvert.DeserializeObject<StreamObject>(jsonString);

        //Get Profile Info
        HttpResponseMessage responseUser = await client.GetAsync($"https://api.twitch.tv/helix/users?login={_config["twitchchannel"]}");
        HttpContent responseUserContent = responseUser.Content;
        string jsonProfileString = await responseUser.Content.ReadAsStringAsync();
        var profile = JsonConvert.DeserializeObject<ProfileObject>(jsonProfileString);

    if (!(stream == default(StreamObject) || stream.data.Count == 0))
    {
        Console.WriteLine($"Live: {stream.data[0].type}");
        Console.WriteLine($"Streamer: {stream.data[0].user_name}");
        Console.WriteLine($"Title: {stream.data[0].title}");
        Console.WriteLine($"Game: {stream.data[0].game_name}");
        Console.WriteLine($"ThumbnailURL: {stream.data[0].thumbnail_url.Replace("{width}x{height}","1920x1080")}");
        Console.WriteLine($"Viewer Count: {stream.data[0].viewer_count}");
        
        //Create Embed
        var embed = new EmbedBuilder{};
        embed.WithFooter(footer => footer.Text = "CentricX")
            .WithTitle($"{stream.data[0].title}")
            .WithAuthor($"{stream.data[0].user_name} is now live on Twitch!", $"{profile.data[0].profile_image_url}")
            
            .WithImageUrl($"{stream.data[0].thumbnail_url.Replace("{width}x{height}","1920x1080")}")
            .WithDescription($"Playing {stream.data[0].game_name} for {stream.data[0].viewer_count} viewers \n [Watch Stream](https://twitch.tv/{stream.data[0].user_name})")
            .WithColor(Color.Blue)
            .WithUrl($"https://twitch.tv/{stream.data[0].user_name}")
            .WithCurrentTimestamp();
        //Send Embed to channel
        await _client.GetGuild(205388858265698304).GetTextChannel(758473454449459260).SendMessageAsync(embed: embed.Build());

    }
        else
    {
        Console.WriteLine("Not Live");
    };
        _logger.LogInformation("Twitch Status geladen");
            
    }
    }
}