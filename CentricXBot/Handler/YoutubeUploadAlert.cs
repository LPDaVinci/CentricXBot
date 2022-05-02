using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CentricxBot.Data;

namespace CentricXBot.Handler
{
    public class YoutubeUploadAlert
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private string oldID;


    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class Id
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }

    public class Default
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

   

    public class Thumbnails
    {
        public Default @default { get; set; }
    }

    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string liveBroadcastContent { get; set; }
        public DateTime publishTime { get; set; }
    }

    public class Item
    {
        public Id id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Root
    {
        public List<Item> items { get; set; }
    }


        public YoutubeUploadAlert(IServiceProvider services)
        {
            System.Timers.Timer timer = new System.Timers.Timer(60000); 
           timer.Elapsed += async ( sender, e ) => await YoutubeAlert(); 
            timer.Start();

            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

             _client.Ready += YoutubeAlert;
        }
        public async Task YoutubeAlert()
        {   
            var clientid = BaseConfig.GetConfig().YoutubeAPIKey;
            var ChannelYT = BaseConfig.GetConfig().YoutubeChannelID;
            
            //Create new HttpClient
            var client = new HttpClient();
            //Send client-id and oauth token to Twitch API
            //Get Stream Info
            HttpResponseMessage response = await client.GetAsync($"https://www.googleapis.com/youtube/v3/search?part=snippet&channelId={ChannelYT}&maxResults=1&order=date&type=video&key={clientid}");
            HttpContent responseContent = response.Content;
            string jsonString = await response.Content.ReadAsStringAsync();
            
            var youtube = JsonConvert.DeserializeObject<Root>(jsonString);

    if (!(youtube == default(Root) || youtube.items.Count == 0))
    {
            if (oldID == null){
                    if (youtube.items[0].id.videoId != null) {
                        oldID = youtube.items[0].id.videoId;
                    }  
                }

            if (youtube.items[0].id.videoId == oldID)
                {
                    Console.WriteLine("old id same as new id");
                } else {
                            Console.WriteLine("newid differs from oldid");
                            oldID = youtube.items[0].id.videoId;
                        } 
                }
            }
        
           
                //Create Embed
                    //var embed = new EmbedBuilder{};
                    //embed.WithFooter(footer => footer.Text = "CentricX")
                    //    .WithTitle($"{youtube.items[0].snippet.title}")
                    //    .WithAuthor($"TGest")
                    //    .WithDescription($"test")
                    //    .WithColor(Color.Blue)
                    //    .WithCurrentTimestamp();
                    //Send Text Before Embed:

                    //Send Embed to channel
    
                ////    var sendchannel = _client.GetChannel(958338678286065706) as IMessageChannel; 
                ////    var msg = await sendchannel.SendMessageAsync(embed: embed.Build()); 
       
                    

            }
            }
    



           
 
   
    
