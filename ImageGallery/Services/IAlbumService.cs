using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallery.Models;
using Microsoft.AspNetCore.Http;

namespace ImageGallery.Services {
    public interface IAlbumService {
        Task<string> AddAlbum(string name);
        Task<Album[]> GetAlbumPreviews();
        Task<Album[]> GetAlbumsWithoutImages();
        Task<Album> GetAlbum(string id);
        Task<Album> GetAlbumMetadata(string id);
        Task AddImagesToAlbumWithId(string id, List<IFormFile> files);
        Task Delete(string albumId, string imageId);
        Task<byte[]> GetThumbnail(string albumId, string imageId);
        Task<byte[]> GetPreRenders(ushort width, ushort height);
        Task<byte[]> GetImage(string albumId, string imageId);
        Task<Image> GetImageWithMetadata(string albumId, string imageId);
    }
}
