using System.IO;

namespace ImageGallery.Services
{
    public interface IImageService
    {

        string CreatePreRender(byte[] imageData, Stream stream);
    }
}
