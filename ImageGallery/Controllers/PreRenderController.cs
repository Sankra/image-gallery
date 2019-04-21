using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PreRenderController : ControllerBase
    {
        readonly IAlbumService albumService;

        public PreRenderController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{width}/{height}")]
        public async Task<IActionResult> Get(string width, string height)
        {
            var image = await albumService.GetPreRenders(ushort.Parse(width), ushort.Parse(height));
            // TODO: content type  from something
            return File(image, "image/png");
        }
    }
}
