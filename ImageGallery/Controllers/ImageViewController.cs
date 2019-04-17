using System.Threading.Tasks;
using ImageGallery.Models;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    public class ImageViewController : Controller
    {
        readonly IAlbumService albumService;

        public ImageViewController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{albumId}/{imageId}")]
        public IActionResult Index(string albumId, string imageId) =>
            View(new FullScreenImage(albumId, imageId));

        [HttpPost("{albumId}/{imageId}")]
        public async Task<IActionResult> Delete(string albumId, string imageId)
        {
            await albumService.Delete(albumId, imageId);
            return RedirectToAction("Index", "Album", new { id = albumId });
        }
    }
}
