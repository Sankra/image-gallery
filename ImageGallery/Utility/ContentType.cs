using System.IO;

namespace ImageGallery.Utility
{
    public static class ContentType
    {
        public static string GetFromFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.Remove(0, 1);
        }
    }
}
