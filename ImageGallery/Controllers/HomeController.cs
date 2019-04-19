using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Configuration;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageGallery.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        readonly IAlbumService albumService;
        readonly IConfiguration configuration;

        public HomeController(IAlbumService albumService, IConfiguration configuration)
        {
            // TODO: Gråe ut alle klikkbare menypunkter når  musen er over dem
            // TODO: Gjør noe smartere med paddingen jeg slenger rundt med i alle views
            this.albumService = albumService;
            this.configuration = configuration;
        }

        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = configuration["Customization:SiteName"];
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
