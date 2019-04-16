namespace ImageGallery.Models
{
    public readonly struct FullScreenImage
    {
        public FullScreenImage(string albumId, string imageId)
        {
            AlbumId = albumId;
            ImageId = imageId;
        }

        public string AlbumId { get; }
        public string ImageId { get; }
    }
}
