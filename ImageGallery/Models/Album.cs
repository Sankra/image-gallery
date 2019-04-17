using System;
using System.Collections.Generic;

namespace ImageGallery.Models
{
    public readonly struct Album : IEquatable<Album>
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

        public bool Equals(Album other) => Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj) => obj is Album other && Equals(other);

        public void AddImage(Image image)
        {
            Images.Add(image);
            Images.Sort();
        }

        public void OrderByDateTaken() => Images.Sort();

        public void Delete(Image image) => Images.Remove(image);
    }
}
