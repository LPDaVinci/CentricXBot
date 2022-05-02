using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using CentricxBot.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using CentricXBot.Functions;


namespace CentricXBot.Handler
{
    public class GuildJoinHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public GuildJoinHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();

            _client.UserJoined += HandleUserJoinedAsync;
            _client.UserLeft += HandleUserLeftAsync;
        }
            public async Task HandleUserJoinedAsync(SocketGuildUser user)
            {
                var botrole = BaseConfig.GetConfig().BotRole;
                
                if (user.IsBot)
                {
                    await user.AddRoleAsync(Convert.ToUInt64($"{botrole}"));
                }
                    var channel = _client.GetChannel(958338678286065706) as SocketTextChannel; // Gets the channel to send the message in

                    

                    await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name}"); //Welcomes the new user
                    _logger.LogInformation($"User [{user.Mention}] ist dem Server beigetreten.");

                    HttpClient httpClient = new HttpClient(); 
                    HttpResponseMessage response = null;
                    SixLabors.ImageSharp.Image<Rgba32> image = null; 
                    response = await httpClient.GetAsync(user.GetAvatarUrl()); /*sets the response to the users avatar*/
                    Stream inputStream = await response.Content.ReadAsStreamAsync(); /*creates a inputStream variable and reads the url*/
    
                    image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream); /*Loads the image to the ImageSharp image we created earlier*/
                    var finimg = ImageSharpFunctions.CreateWelcomeImage(image, user.Nickname +"#"+ user.Discriminator, user.Guild.MemberCount );  
                    using (System.IO.MemoryStream imgStream = new System.IO.MemoryStream(finimg))
                    {
                        await channel.SendFileAsync(imgStream, "anyImageName.png", "", false);

                    } 

            }
                public async Task HandleUserLeftAsync(SocketGuild guild, SocketUser user)
                {
                    var channel = _client.GetChannel(958338678286065706) as SocketTextChannel; // Gets the channel to send the message in
                    await channel.SendMessageAsync($"Bye {user.Mention} to {guild.Name}"); //Welcomes the new user
                _logger.LogInformation($"User [{user.Mention}] hat den Server verlassen.");
                }

    }
}
