using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Models;
using ImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageGallery.Controllers
{
    [Route("/[controller]")]
    public class AlbumController : Controller
    {
        readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index(string id)
        {
            // TODO: Show latest from all albums, randomized order
            var album = await albumService.GetAlbum(id);
            ViewData["Title"] = album.Name;
            return View(album);
        }

        //  TODO: Move upload to own page
        [HttpPost("{id}")]
        public async Task<IActionResult> Index(string id, List<IFormFile> files)
        {
            await albumService.AddImagesToAlbumWithId(id, files);
            return await Index(id);
        }
    }
}
