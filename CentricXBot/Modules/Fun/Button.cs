using Discord;
using Discord.Commands;


namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Button : ModuleBase<SocketCommandContext>
    {
        [Command("button")]
        public async Task ButtonCommand()
        {
            var myReaction = new Emoji("👍");
            var SecondEmoji = Emote.Parse("<:youtube:896785526164684810>");

            var builder = new ComponentBuilder()
                .WithButton("Label 1", "test-1", ButtonStyle.Success, myReaction )
                .WithButton("Label 2", "test-2", ButtonStyle.Success, SecondEmoji);

            var exampleFooter = new EmbedFooterBuilder()
                .WithText("CentricX © 2022")
                .WithIconUrl("https://static-cdn.jtvnw.net/jtv_user_pictures/24447486-ab3e-4871-8ecd-67c5e5cb69d1-profile_image-70x70.png");
            var embed = new EmbedBuilder{};
            embed.WithFooter(exampleFooter)
            .WithTitle("Neue Support Nachricht")
            .WithDescription($"**CentricX**\n\nWelche Option willst du ausführen?")
            .WithColor(Color.Blue);

            var msg = await Context.Channel.SendMessageAsync("",false, embed.Build(), components: builder.Build());
            await Task.Delay(2000);
            await Context.Message.DeleteAsync();
        }         
    }
}
