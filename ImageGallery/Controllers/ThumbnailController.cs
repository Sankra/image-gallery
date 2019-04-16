using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Services;
using ImageGallery.Utility;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    public class ThumbnailController : Controller
    {
        readonly IAlbumService albumService;

        public ThumbnailController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{id}/{imageId}")]
        public async Task<IActionResult> Get(string id, string imageId)
        {
            var image = await albumService.GetThumbnail(id, imageId);
            return File(image, "image/" + ContentType.GetFromFileName(imageId));
        }
    }
}
