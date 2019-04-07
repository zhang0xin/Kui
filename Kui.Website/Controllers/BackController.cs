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
    public class BackController : Controller
    {
        ConfigService _configService;
        public BackController(ConfigService configService)
        {
            _configService = configService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}