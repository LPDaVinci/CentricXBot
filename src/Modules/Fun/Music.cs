using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Lavalink4NET;
using Lavalink4NET.Rest;
using Lavalink4NET.Player;

namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Music : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService _audioService;

         public Music(IAudioService audioService)
        => _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        
[Command("play", RunMode = RunMode.Async)]
    public async Task Play([Remainder] string query)
    {
        var player = await GetPlayerAsync();

        if (player == null)
        {
            return;
        }

        var track = await _audioService.GetTrackAsync(query, SearchMode.YouTube);

        if (track == null)
        {
            await ReplyAsync("😖 No results.");
            return;
        }

        var position = await player.PlayAsync(track, enqueue: true);

        if (position == 0)
        {
            await ReplyAsync("🔈 Playing: " + track.Source);
        }
        else
        {
            await ReplyAsync("🔈 Added to queue: " + track.Source);
        }
    }
     private async ValueTask<VoteLavalinkPlayer> GetPlayerAsync(bool connectToVoiceChannel = true)
    {
        var player = _audioService.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);

        if (player != null
            && player.State != PlayerState.NotConnected
            && player.State != PlayerState.Destroyed)
        {
            return player;
        }

        var user = Context.Guild.GetUser(Context.User.Id);

        if (!user.VoiceState.HasValue)
        {
            await ReplyAsync("You must be in a voice channel!");
            return null;
        }

        if (!connectToVoiceChannel)
        {
            await ReplyAsync("The bot is not in a voice channel!");
            return null;
        }

        return await _audioService.JoinAsync<VoteLavalinkPlayer>(user.Guild.Id, user.VoiceChannel.Id);
    }
    }
}