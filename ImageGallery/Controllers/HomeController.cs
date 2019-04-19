using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageGallery.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        readonly IAlbumService albumService;

        public HomeController(IAlbumService albumService)
        {
            // TODO: Favicon
            this.albumService = albumService;
        }

        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "DIPS Photos";
            var albumPreviews = await albumService.GetAlbumPreviews();
            return View(albumPreviews);
        }
    }
}
