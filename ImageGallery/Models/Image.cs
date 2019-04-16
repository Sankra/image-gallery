namespace ImageGallery.Models
{
    public readonly struct Image
    {
        public Image(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
