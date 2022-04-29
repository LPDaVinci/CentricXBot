using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using CentricXBot.Functions;

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
            response = await httpClient.GetAsync(Context.Client.CurrentUser.GetAvatarUrl()); /*sets the response to the users avatar*/
            Stream inputStream = await response.Content.ReadAsStreamAsync(); /*creates a inputStream variable and reads the url*/
    
            image = SixLabors.ImageSharp.Image.Load<Rgba32>(inputStream); /*Loads the image to the ImageSharp image we created earlier*/
            var finimg = ImageSharpFunctions.CreateRoundedImage(image);  
            using (MemoryStream imgStream = new MemoryStream(finimg))
                {
                    await Context.Channel.SendFileAsync(imgStream, "test.png");
                } 
        }
    }
    
    

        }

    
