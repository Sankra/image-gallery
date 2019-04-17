using System;

namespace ImageGallery.Models
{
    public readonly struct Image : IEquatable<Image>, IComparable
    {
        public Image(string id, DateTime dateTaken, ushort width, ushort height)
        {
            Id = id;
            DateTaken = dateTaken;
            Width = width;
            Height = height;
        }

        public string Id { get; }
        public DateTime DateTaken { get; }
        public ushort Width { get; }
        public ushort Height { get; }

        public bool Equals(Image other) => Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj) => obj is Image other && Equals(other);

        public int CompareTo(object obj) => ((Image)obj).DateTaken.CompareTo(DateTaken);
    }
}
