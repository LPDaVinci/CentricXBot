using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using CentricxBot.Data;
using Newtonsoft.Json.Linq;

namespace CentricXBot.Handler
{
    public class VoiceStateHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public VoiceStateHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();

            _client.UserVoiceStateUpdated += HandleVoiceState;

        }
        public async Task HandleVoiceState(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            var autocreatechannelid = BaseConfig.GetConfig().JoinToCreateChannel;
            var autocreatecatid = BaseConfig.GetConfig().TempVoiceCategory;
  
            Console.WriteLine($"VoiceStateUpdate: {user} - {before.VoiceChannel?.Name ?? "null"} -> {after.VoiceChannel?.Name ?? "null"}");
            if (user is SocketGuildUser socketGuildUser)
            {
                var server = socketGuildUser.Guild;
                var autocreateid = Convert.ToUInt64($"{autocreatechannelid}");
                var autocreatecategoryid = Convert.ToUInt64($"{autocreatecatid}");
                bool userWannaCreateTempChannel = socketGuildUser.VoiceState?.VoiceChannel.Id == autocreateid;
                    
                if (userWannaCreateTempChannel) //Join to Create Channel
                {
                    //Create VC
                    var voiceChannel = await server.CreateVoiceChannelAsync(socketGuildUser.Username, prop => prop.CategoryId = autocreatecategoryid);
                    //Gives User Full Permissions over his Channel
                    await voiceChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(connect: PermValue.Allow, viewChannel: PermValue.Allow, manageChannel: PermValue.Allow));
                    //Move User in his Temp Channel
                    await socketGuildUser.ModifyAsync(x =>
                    {
                        x.Channel = Optional.Create(voiceChannel as IVoiceChannel);
                    });
                }
                var usersVoiceChannel = (socketGuildUser as IVoiceState).VoiceChannel;
                if (usersVoiceChannel == null)
                {
                    if ((after.VoiceChannel != before.VoiceChannel)
                        && before.VoiceChannel.Category.Id.Equals(autocreatecategoryid) 
                        && before.VoiceChannel.ConnectedUsers.Count == 0
                        && !before.VoiceChannel.Id.Equals(autocreateid))
                    {
                        await before.VoiceChannel.DeleteAsync();
                    } 
                }else{
                        if (!(before.VoiceChannel == null)
                            && before.VoiceChannel.Category.Id.Equals(autocreatecategoryid) 
                            && before.VoiceChannel.ConnectedUsers.Count == 0
                            && !before.VoiceChannel.Id.Equals(autocreateid))
                        { 
                            await before.VoiceChannel.DeleteAsync();
                        } 
                    }            
                            
               } 
        }
    } 
}




