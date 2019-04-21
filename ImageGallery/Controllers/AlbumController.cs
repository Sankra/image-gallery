using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    public class AlbumController : NavigableController
    {
        readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService) : base(albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{albumId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index(string albumId)
        {
            await SetMenuItems();
            var album = await albumService.GetAlbum(albumId);
            ViewData["Title"] = album.Name;
            ViewData["AlbumId"] = albumId;
            ViewData["AddUrl"] = $"/Album/{albumId}/Add";
            return View(album);
        }

        [HttpGet("{albumId}/Add")]
        public async Task<IActionResult> Add(string albumId)
        {
            // TODO: Gjør noe med at ViewData tittel er overalt
            // TODO: Gjør det mulig å laste opp mange flere  bilder enn er mulig i dag
            await SetMenuItems();
            var album = await albumService.GetAlbumMetadata(albumId);
            ViewData["Title"] = album.Name;
            ViewData["AlbumId"] = albumId;
            return View("Add");
        }

        [HttpPost("{albumId}/Add")]
        public async Task<IActionResult> AddFiles(string albumId, List<IFormFile> photos)
        {
            // TODO: Errorhandling
            await albumService.AddImagesToAlbumWithId(albumId, photos);
            //  TODO: implement in js
            return RedirectToAction("Index", "Album", new { albumId });
        }
    }
}
