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
            // TODO: Gråe ut alle klikkbare menypunkter når  musen er over dem
            // TODO: Gjør noe smartere med paddingen jeg slenger rundt med i alle views
            this.albumService = albumService;
        }

        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "DIPS Photos";
            ViewData["ShowAdd"] = true;
            var albumPreviews = await albumService.GetAlbumPreviews();
            return View(albumPreviews);
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            // TODO: Gjør noe med at tittel er overalt
            ViewData["Title"] = "Create New Album";
            return View("Add");
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(string name)
        {
            // TODO: Error handling
            var albumId = await albumService.AddAlbum(name);
            return RedirectToAction("Index", "Album", new { albumId });
        }
    }
}
