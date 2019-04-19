﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{albumId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Index(string albumId)
        {
            // TODO: Show latest from all albums, randomized order
            var album = await albumService.GetAlbum(albumId);
            ViewData["Title"] = album.Name;
            ViewData["AlbumId"] = albumId;
            ViewData["ShowAdd"] = true;
            return View(album);
        }

        [HttpGet("{albumId}/Add")]
        public async Task<IActionResult> Add(string albumId)
        {
            // TODO: Gjør noe med at tittel er overalt
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
            return RedirectToAction("Index", "Album", new { albumId });
        }
    }
}
