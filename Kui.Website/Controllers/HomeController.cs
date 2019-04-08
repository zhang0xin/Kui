using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kui.Website.Models;
using Kui.Website.Services;
using Kui.Core.Resource.Node;

namespace Kui.Website.Controllers
{
    public class HomeController : Controller
    {
        ResourceService _resourceService;
        public HomeController(ResourceService kuiSiteService)
        {
            _resourceService = kuiSiteService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Home/List/{**path}")]
        public IActionResult List(string path)
        {
            var nodes = _resourceService.Get<PageNode>(path);

            ViewBag.path = path;
            return View();
        }
        [Route("/Home/{path}/Page/{pageSize}/{pageIndex}")]
        public IActionResult Page(int pageSize, int pageIndex, string path)
        {
            var nodes = _resourceService.Get<PageNode>(path);

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
