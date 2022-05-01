// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CentricXBot.Functions
{
    public static class ImageSharpFunctions
    {

        // Implements a full image mutating pipeline operating on IImageProcessingContext
        public static IImageProcessingContext ConvertToAvatar(this IImageProcessingContext processingContext, Size size, float cornerRadius)
        {
            return processingContext.Resize(new ResizeOptions
            {
                Size = size,
                Mode = ResizeMode.Crop
            }).ApplyRoundedCorners(cornerRadius);
        }


        // This method can be seen as an inline implementation of an `IImageProcessor`:
        // (The combination of `IImageOperations.Apply()` + this could be replaced with an `IImageProcessor`)
        public static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext ctx, float cornerRadius)
        {
            Size size = ctx.GetCurrentSize();
            IPathCollection corners = BuildCorners(size.Width, size.Height, cornerRadius);

            ctx.SetGraphicsOptions(new GraphicsOptions()
            {
                Antialias = true,
                AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // enforces that any part of this shape that has color is punched out of the background
            });
            
            // mutating in here as we already have a cloned original
            // use any color (not Transparent), so the corners will be clipped
            foreach (var c in corners)
            {
                ctx = ctx.Fill(Color.Red, c);
            }
            return ctx;
        }

        public static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
        {
            // first create a square
            var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

            // then cut out of the square a circle so we are left with a corner
            IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

            // corner is now a corner shape positions top left
            //lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

            float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
            float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

            // move it across the width of the image - the width of the shape
            IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
            IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
            IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

            return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
        }

                public static byte[] CreateRoundedImage(SixLabors.ImageSharp.Image myImage, string username, int count)
            {
                FontCollection collection = new();
                FontFamily family = collection.Add("fonts/Rubik-Black.ttf");
                Font font = family.CreateFont(16, FontStyle.Italic);

                int bgWidth = 450;
                int bgHeight = 200;

                string JoinedMember = $"{username} joined the Server.";
                string CountMember = $"Member #{count}";

                SixLabors.ImageSharp.Image background = new Image<Rgba32>(bgWidth, bgHeight, Color.Black);
                SixLabors.ImageSharp.Image avatar = myImage.Clone(x => x.ConvertToAvatar(new Size(200, 200), 100));
                Vector2 center = new Vector2(avatar.Width/2 - 10, background.Height/2 + 25); //center horizontally, 10px down 
                Vector2 center2 = new Vector2(background.Width/2 - 50, background.Height/2 + 50); //center horizontally, 10px down 


                    using (MemoryStream stream = new MemoryStream())
                    {                   
                        avatar.Mutate(x => x.Resize(100, 100));
                        background.Mutate(x => x.DrawImage(avatar, new Point(((background.Width - avatar.Width) / 2), ((background.Height-avatar.Height) / 6)), opacity: 1.0f));
                        background.Mutate(x=> x.DrawText(JoinedMember, font, Color.White, center));
                        background.Mutate(x=> x.DrawText(CountMember, font, Color.White, center2));
                                              
                        background.Save(stream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                        stream.Seek(0, SeekOrigin.Begin);
                        background.Dispose();    
                                                 
                        return stream.ToArray();       
                    }
            }


    }
}