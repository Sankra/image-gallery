using System.Threading.Tasks;
using ImageGallery.Models;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers {
    [Route("/[controller]")]
    public class ImageViewController : NavigableController {
        readonly IAlbumService albumService;

        public ImageViewController(IAlbumService albumService) : base(albumService) =>
            this.albumService = albumService;

        [HttpGet("{albumId}/{imageId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index(string albumId, string imageId) {
            await SetMenuItems();
            var image = await albumService.GetImageWithMetadata(albumId, imageId);
            ViewData["Title"] = image.DateTaken;
            ViewData["AlbumId"] = albumId;
            ViewData["ShowDelete"] = true;
            ViewData["AddUrl"] = $"/Album/{albumId}/Add";
            return View(new FullScreenImage(albumId, imageId));
        }


        [HttpDelete("{albumId}/{imageId}")]
        public async Task<IActionResult> Delete(string albumId, string imageId) {
            await albumService.Delete(albumId, imageId);
            return NoContent();
        }
    }
}
