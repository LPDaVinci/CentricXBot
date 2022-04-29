using System.Net.Http;
using CentricxBot.Data;
using Discord;
using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using CentricXBot.Functions;
using System.Numerics;

namespace CentricXBot.Modules.Fun
{
    public class ImageTest : ModuleBase<SocketCommandContext>
    {
        [Command("image")]
        public async Task ImageCommand()
        {
            HttpClient httpClient = new HttpClient(); 
            HttpResponseMessage response = null;
            SixLabors.ImageSharp.Image<Rgba32> image = null; 
            response = await httpClient.GetAsync(Context.User.GetAvatarUrl()); /*sets the response to the users avatar*/
            Stream inputStream = await response.Content.ReadAsStreamAsync(); /*creates a inputStream variable and reads the url*/
            image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream); /*Loads the image to the ImageSharp image we created earlier*/
            inputStream.Dispose();
            Stream outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);
            outputStream.Position = 0;
            var file = File.Create("test.png");
            await outputStream.CopyToAsync(file);
            file.Dispose();
                // as generate returns a new IImage make sure we dispose of it
                using (SixLabors.ImageSharp.Image destRound = image.Clone(x => x.ConvertToAvatar(new Size(200, 200), 100)))
                {
                    destRound.Save("test.png");
                }


            await Context.Channel.SendFileAsync($"test.png");
            File.Delete("test.png");

        }
    }
    
    

        }

    
