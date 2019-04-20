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

        public ImageViewController(IAlbumService albumService) =>
            this.albumService = albumService;

        [HttpGet("{albumId}/{imageId}")]
        public async Task<IActionResult> Index(string albumId, string imageId)
        {
            var album = await albumService.GetAlbum(albumId);
            ViewData["Title"] = album.Name;
            ViewData["AlbumId"] = albumId;
            ViewData["ShowDelete"] = true;
            ViewData["AddUrl"] = $"/Album/{albumId}/Add";
            return View(new FullScreenImage(albumId, imageId));
        }


        [HttpDelete("{albumId}/{imageId}")]
        public async Task<IActionResult> Delete(string albumId, string imageId)
        {
            await albumService.Delete(albumId, imageId);
            return NoContent();
        }
    }
}
