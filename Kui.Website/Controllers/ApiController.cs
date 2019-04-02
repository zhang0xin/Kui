using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Node;
using Kui.Website.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kui.Website.Controllers 
{
    public class ApiController : Controller
    {
        ConfigService _configService;
        public ApiController(ConfigService configService)
        {
            _configService = configService;
        }

        public IActionResult GetSiteNodes()
        {
            return Json(_configService.GetSiteNodes());
        }
        public IActionResult AddSiteNode(BaseNode node)
        {
            throw new NotImplementedException();
        }
    }
}