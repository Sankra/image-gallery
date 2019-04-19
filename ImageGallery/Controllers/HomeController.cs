using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageGallery.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        readonly IAlbumService albumService;
        readonly IConfiguration configuration;

        public HomeController(IAlbumService albumService, IConfiguration configuration)
        {
            // TODO: Configurerbar farge på menyen
            // TODO: Linkene på knappene skal ikke være JS, men vanlige lenker
            // TODO: Gråe ut alle klikkbare menypunkter når  musen er over dem
            // TODO: Gjør noe smartere med paddingen jeg slenger rundt med i alle views

            this.albumService = albumService;
            this.configuration = configuration;
        }

        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index()
        {
            // TODO: configurable values should not be strings...
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
