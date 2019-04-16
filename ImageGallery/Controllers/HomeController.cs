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


        // GET: /<controller>/
        public IActionResult Index()
        {
            return null;
        }
    }
}
