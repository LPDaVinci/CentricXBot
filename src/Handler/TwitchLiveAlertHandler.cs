using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CentricXBot.Handler
{
    public class TwitchLiveAlertHandler
    {
       
    // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        private bool isLive = false;
        public TwitchLiveAlertHandler(IServiceProvider services)
        {
            //Timer Infinite Loop need to check if not Live still Loop if live and posted dont post any new but still checks
            System.Timers.Timer timer = new System.Timers.Timer(300000); //every 5 Minutes Check
            timer.Elapsed += async ( sender, e ) => TwitchLiveAlert();
            timer.Start();

            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _services = services;

            // hook into these events with the methods provided below
             _client.Ready += TwitchLiveAlert;
        }
        
    public class TwitchJsonData
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
        public List<TwitchJsonData> data { get; set; }

    }   
    public class ProfileTwitch
    {
        [JsonProperty("profile_image_url")] public string profile_image_url { get; set; }
    }

    public class ProfileObject
    {
        public List<ProfileTwitch> data { get; set; }
    }

    public async Task TwitchLiveAlert()
    {
        
        //Create new HttpClient
        var client = new HttpClient();
        //Send client-id and oauth token to Twitch API
        client.DefaultRequestHeaders.Add("Client-ID", $"{_config["clientid"]}"); 
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{_config["oauth"]}");
       //Get Stream Info
        HttpResponseMessage response = await client.GetAsync($"https://api.twitch.tv/helix/streams?user_login={_config["live-alert-streamer"]}");
        HttpContent responseContent = response.Content;
        string jsonString = await response.Content.ReadAsStringAsync();
        var stream = JsonConvert.DeserializeObject<StreamObject>(jsonString);

        //Get Profile Info just for the profile picture
        HttpResponseMessage responseUser = await client.GetAsync($"https://api.twitch.tv/helix/users?login={_config["live-alert-streamer"]}");
        HttpContent responseUserContent = responseUser.Content;
        string jsonProfileString = await responseUser.Content.ReadAsStringAsync();
        var profile = JsonConvert.DeserializeObject<ProfileObject>(jsonProfileString);

    //Check if the channel is not offline
    if (!(stream == default(StreamObject) || stream.data.Count == 0))
    {
        if (isLive == false) {

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
            ulong ChannelID = Convert.ToUInt64(_config["live-alert-channel"]);
            var sendchannel = _client.GetChannel(ChannelID) as IMessageChannel; 
            var msg = await sendchannel.SendMessageAsync(embed: embed.Build()); 
            isLive = true;
        }
        else {
            return;
        }
    }
        //If channel is offline do
        else
    {
        if (isLive == true) {
        isLive = false;
    }
    }
    }
    }
}
