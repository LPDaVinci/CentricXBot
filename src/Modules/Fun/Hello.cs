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

namespace CentricXBot.Modules.Fun
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    [Summary("The Game Module")]
        public class Hello : ModuleBase
    {
        [Command("hello")]
        [Summary("PrintsHello")]
        [Alias("testy")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage ="You don't have the permission ``ban_member``!")]
        public async Task HelloCommand()
        {
            //Footer
            var exampleFooter = new EmbedFooterBuilder()
                .WithText("CentricX © 2022")
                .WithIconUrl("https://static-cdn.jtvnw.net/jtv_user_pictures/24447486-ab3e-4871-8ecd-67c5e5cb69d1-profile_image-70x70.png");
            var embed = new EmbedBuilder{};
                embed.WithFooter(exampleFooter)
                .WithTitle($"**__Willkommen auf dem DC Server von LPDaVinci__**")
                .WithImageUrl("https://lh4.googleusercontent.com/AOnSz48qnXq7nF_NumrwaSVkj30X6Mu_PJ5nHVneShrvUM7xMSh3ZIyHWoevXHial2ipRk5IZ4nckwDRluN6X4V89vdc8Osz45CHkLHWRN-2bXvXXe5ycRc12gvlGLoCSA")
                .WithDescription($"LPDaVinci ist ein deutscher Content-Creator und Influencer.\n" + 
                "Seit November 2010 ist er unter dem Namen LPDaVinci bekannt.\n\u200b\n\u200b" +
                "Lies dir bitte die Regeln durch.\n\u200b" +
                "<:pfeil:896785525522956338> <#205388858265698304>\n\u200b\n\u200b" +
                "Gebe dir selbst einige Rollen.\n\u200b"+
                "<:pfeil:896785525522956338> <#798207431254868029>\n\u200b\n\u200b"+
                "Du bist wegen Fortnite Customs hier dann registrier dich mit unserem Yunite Bot.\n\u200b" +
                "<:pfeil:896785525522956338> <#808055416092753922>\n\u200b\n\u200b" +

                "**Soziale Medien und Links**\n\u200b\n\u200b" +
                "<:twitter:896785527108415538> [Twitter](https://twitter.com/lpdavinci)\n\u200b\n\u200b" +
                "<:youtube:896785526164684810> [Youtube](https://youtube.com/lpdavinci)\n\u200b\n\u200b" +
                "<:instagram:896785526605119519> [Instagram](https://instagram.com/lpdavinci)\n\u200b\n\u200b" +
                "<:tiktok:896785525678170165> [TikTok](https://tiktok.com/@lpdavinci)\n\u200b\n\u200b" +
                "<:discord:896785525552345159> [Discord](https://discord.gg/WGFhwFJAbf)\n\u200b\n\u200b" +
                "<:twitch:896785527322333304> [Twitch](https://twitch.tv/lpdavinci)\n\u200b\n\u200b" +

                "**Du möchtest mich unterstützen:**\n\u200b\n\u200b" +
                "<:patreon:958139720997683200> [Patreon](https://www.patreon.com/lpdavinci)\n\u200b\n\u200b" +
                "**Moderator Bewerbungen unter:**\n\u200b" +

                "\n> <:pfeil:896785525522956338> **Bewerbung als Moderator:**	[HIER KLICKEN](https://forms.gle/LA2o8PA9KUFJFSzY7)" +
                "\n> <:pfeil:896785525522956338> **Entbannungsantrag:**	[HIER KLICKEN](https://forms.gle/p4rLEnUMx1SDZSRw5)")
                .WithColor(Color.Blue);

        
            //Send Embed to channel
            await ReplyAsync(embed: embed.Build());
        }

       
    }
}