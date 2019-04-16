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
    }
}
