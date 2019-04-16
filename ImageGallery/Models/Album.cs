using System.Collections.Generic;

namespace ImageGallery.Models
{
    public readonly struct Album
    {
        public Album(string id, string name)
        {
            Id = id;
            Name = name;
            Images = new List<Image>();
        }

        public string Id { get; }
        public string Name { get; }
        public List<Image> Images { get; }
    }
}
