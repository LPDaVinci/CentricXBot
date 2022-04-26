using Discord;
using Discord.Commands;
using Fergun.Interactive;

namespace CentricXBot.Modules.Fun
{
   
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class Embed : ModuleBase<SocketCommandContext>
    {
        public InteractiveService Interactive { get; set; }
        private static ulong id;
        [Command("embed", RunMode = RunMode.Async)]
    public async Task NextMessageAsync()
    {   
         var embed = new EmbedBuilder()
                {
                    Title = $"Willkommen beim Embed Generator!",
                    Description = "Gebe zuerst einen Titel ein!",
                                
                }.Build();
        var msg = await ReplyAsync(embed: embed);

        // Wait for a message in the same channel the command was executed.
        var title = await Interactive.NextMessageAsync(x => x.Channel.Id == Context.Channel.Id, timeout: TimeSpan.FromSeconds(13));

        await msg.ModifyAsync(x => {
        if (title.IsSuccess){

            x.Embed = new EmbedBuilder()
            .WithColor(new Color(40, 40, 120))
            .WithTitle($"{title.Value.Content}")
            .WithDescription($"Der neue Titel lautet {title.Value.Content}\nGebe nun die Beschreibung ein!")
            .Build(); //<-- The is what was omitted.
            title.Value.DeleteAsync();
            } else
                {

            x.Embed = new EmbedBuilder()
            .WithColor(new Color(40, 40, 120))
            .WithTitle("Embed Creation Error")
            .WithDescription($"Folgender Fehler ist aufgetreten: {title.Status} <- Du hast zu lange gebraucht einen Titel einzugeben.")
            .Build(); //<-- The is what was omitted.
            
        }
        });

        var description = await Interactive.NextMessageAsync(x => x.Channel.Id == Context.Channel.Id, timeout: TimeSpan.FromSeconds(13));

        await msg.ModifyAsync(x => {
        if (description.IsSuccess){
            x.Embed = new EmbedBuilder()
            .WithColor(new Color(40, 40, 120))
            .WithTitle($"{title.Value.Content}")
            .WithDescription($"{description.Value.Content}\n Gebe als nächstes ein, wo das Embed gepostet werden soll")
            .Build(); //<-- The is what was omitted.
            description.Value.DeleteAsync();
            } else
                {
            x.Embed = new EmbedBuilder()
            .WithColor(new Color(40, 40, 120))
            .WithTitle("Embed Creation Error")
            .WithDescription($"Folgender Fehler ist aufgetreten: {description.Status} <- Du hast zu lange gebraucht eine Beschreibung einzugeben.")
            .Build(); //<-- The is what was omitted.
        }
        });

        var postchannel = await Interactive.NextMessageAsync(x => x.Channel.Id == Context.Channel.Id, timeout: TimeSpan.FromSeconds(13));
        if (postchannel.IsSuccess){

                var channelstuff = MentionUtils.TryParseChannel(postchannel.Value.Content, out id);
                var chn = await Context.Client.GetChannelAsync(id) as IMessageChannel;
    
                chn.SendMessageAsync("test");
                postchannel.Value.DeleteAsync();
            } else {
                await msg.ModifyAsync(x => {
                    {
            x.Embed = new EmbedBuilder()
            .WithColor(new Color(40, 40, 120))
            .WithTitle("Embed Creation Error")
            .WithDescription($"Folgender Fehler ist aufgetreten: {postchannel.Status} <- Du hast zu lange gebraucht eine Beschreibung einzugeben.")
            .Build(); //<-- The is what was omitted.
        }
            });
            }
    }         
    }
    }


