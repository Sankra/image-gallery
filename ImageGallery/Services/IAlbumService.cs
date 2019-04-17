using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallery.Models;
using Microsoft.AspNetCore.Http;

namespace ImageGallery.Services
{
    public interface IAlbumService
    {
        Task<Album> GetAlbum(string id);
        Task AddImagesToAlbumWithId(string id, List<IFormFile> files);
        Task Delete(string albumId, string imageId);
        Task<byte[]> GetThumbnail(string albumId, string imageId);
        Task<byte[]> GetPreRenders(ushort width, ushort height);
        Task<byte[]> GetImage(string albumId, string imageId);
    }
}
