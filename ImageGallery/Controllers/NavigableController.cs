using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    public abstract class NavigableController : Controller
    {
        readonly IAlbumService albumService;

        protected NavigableController(IAlbumService albumService) =>
            this.albumService = albumService;

        protected async Task SetMenuItems()
        {
            // TODO: cache and reuse menu content
            var albums = await albumService.GetAlbumsWithoutImages();
            ViewData["MenuItems"] = albums;
        }
    }
}
