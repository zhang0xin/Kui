using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kui.Website.Models;

namespace Kui.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(int pageSize, int pageIndex, string path)
        {
            ViewBag.path = path;
            ViewBag.pageSize = pageSize;
            ViewBag.pageIndex = pageIndex;
            return View();
        }
        public IActionResult Item(int id, string path)
        {
            ViewBag.path = path;
            ViewBag.id = id;
            return View();
        }

        public IActionResult Privacy(string path)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
