using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageGallery.Services
{
    public class ImageService : IImageService
    {

        public string CreatePreRender(byte[] imageData, Stream stream)
        {
            using (Image<Rgba32> image = Image.Load(imageData))
            {
                using (var solidImage = new Image<Rgba32>(image.Width, image.Height))
                {
                    solidImage.Mutate(x => x
                        .BackgroundColor(new Rgba32(250, 0, 0)));
                    solidImage.Save(stream, new PngEncoder());
                    return solidImage.Width + "x" + solidImage.Height;
                }
            }
        }
    }
}
