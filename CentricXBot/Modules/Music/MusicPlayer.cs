﻿using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.Rest;
using Lavalink4NET.Player;

namespace CentricXBot.Modules.Music
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class MusicPlayer : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService _audioService;
         public MusicPlayer(IAudioService audioService)
        => _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        
/// <summary>
    ///     Disconnects from the current voice channel connected to asynchronously.
    /// </summary>
    /// <returns>a task that represents the asynchronous operation</returns>
    [Command("disconnect", RunMode = RunMode.Async)]
    public async Task Disconnect()
    {
        var player = await GetPlayerAsync();

        if (player == null)
        {
            return;
        }

        // when using StopAsync(true) the player also disconnects and clears the track queue.
        // DisconnectAsync only disconnects from the channel.
        await player.StopAsync(true);
        await ReplyAsync("Disconnected.");
    }

    /// <summary>
    ///     Plays music from YouTube asynchronously.
    /// </summary>
    /// <param name="query">the search query</param>
    /// <returns>a task that represents the asynchronous operation</returns>
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
            await ReplyAsync("🔈 Playing: " + track.Title);
        }
        else
        {
            await ReplyAsync("🔈 Added to queue: " + track.Title);
        }
    }

    /// <summary>
    ///     Shows the track position asynchronously.
    /// </summary>
    /// <returns>a task that represents the asynchronous operation</returns>
    [Command("position", RunMode = RunMode.Async)]
    public async Task Position()
    {
        var player = await GetPlayerAsync();

        if (player == null)
        {
            return;
        }

        if (player.CurrentTrack == null)
        {
            await ReplyAsync("Nothing playing!");
            return;
        }

        await ReplyAsync($"Position: {player.Position.Position} / {player.CurrentTrack.Duration}.");
    }

    /// <summary>
    ///     Stops the current track asynchronously.
    /// </summary>
    /// <returns>a task that represents the asynchronous operation</returns>
    [Command("stop", RunMode = RunMode.Async)]
    public async Task Stop()
    {
        var player = await GetPlayerAsync();

        if (player == null)
        {
            return;
        }

        if (player.CurrentTrack == null)
        {
            await ReplyAsync("Nothing playing!");
            return;
        }

        await player.StopAsync();
        await ReplyAsync("Stopped playing.");
    }

    /// <summary>
    ///     Updates the player volume asynchronously.
    /// </summary>
    /// <param name="volume">the volume (1 - 1000)</param>
    /// <returns>a task that represents the asynchronous operation</returns>
    [Command("volume", RunMode = RunMode.Async)]
    public async Task Volume(int volume = 100)
    {
        if (volume is > 1000 or < 0)
        {
            await ReplyAsync("Volume out of range: 0% - 1000%!");
            return;
        }

        var player = await GetPlayerAsync();

        if (player == null)
        {
            return;
        }

        await player.SetVolumeAsync(volume / 100f);
        await ReplyAsync($"Volume updated: {volume}%");
    }

    /// <summary>
    ///     Gets the guild player asynchronously.
    /// </summary>
    /// <param name="connectToVoiceChannel">
    ///     a value indicating whether to connect to a voice channel
    /// </param>
    /// <returns>
    ///     a task that represents the asynchronous operation. The task result is the lavalink player.
    /// </returns>
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