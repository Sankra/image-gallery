using ImageGallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    public class ImageViewController : Controller
    {
        [HttpGet("{albumId}/{imageId}")]
        public IActionResult Index(string albumId, string imageId) =>
            View(new FullScreenImage(albumId, imageId));

        [HttpPost("{albumId}/{imageId}")]
        public IActionResult Delete(string albumId, string imageId)
        {
            // TODO: Delete image here...
            return RedirectToAction("Index", "Album", albumId);
        }
    }
}
