using System.Threading.Tasks;
using ImageGallery.Services;
using ImageGallery.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers {
    [Route("/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        readonly IAlbumService albumService;

        public ImageController(IAlbumService albumService) {
            this.albumService = albumService;
        }

        [HttpGet("{albumId}/Random")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Random(string albumId) {
            var album = await albumService.GetAlbum(albumId);
            var randomImage = album.GetRandomImage();
            return await Get(albumId, randomImage.Id);
        }

        [HttpGet("{albumId}/{imageId}")]
        public async Task<IActionResult> Get(string albumId, string imageId) {
            var image = await albumService.GetImage(albumId, imageId);
            return File(image, "image/" + ContentType.GetFromFileName(imageId));
        }
    }
}
