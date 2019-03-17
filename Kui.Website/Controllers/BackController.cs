using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kui.Website.Models;
using Kui.Website.Services;
using Kui.Core.Node;

namespace Kui.Website.Controllers 
{
    public class BackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}