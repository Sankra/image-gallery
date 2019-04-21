using System.Threading.Tasks;
using ImageGallery.Services;
using ImageGallery.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        readonly IAlbumService albumService;

        public ImageController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{id}/{imageId}")]
        public async Task<IActionResult> Get(string id, string imageId)
        {
            var image = await albumService.GetImage(id, imageId);
            return File(image, "image/" + ContentType.GetFromFileName(imageId));
        }
    }
}
