using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Resource.Node;
using Kui.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kui.Website.Controllers 
{
    public class ApiController : Controller
    {
        ConfigService _configService;
        ResourceService _resourceService;
        public ApiController(ConfigService configService, ResourceService resourceService)
        {
            _configService = configService;
            _resourceService = resourceService;
        }

        public IActionResult GetSiteNodeTree()
        {
            return Json(_configService.GetSiteNodes());
        }
        public IActionResult GetSiteNode(string path)
        {
            return Json(_resourceService.GetSubNodes<SiteNode>(path));
        }
        public IActionResult SaveSiteNode()
        {
            var param = ReadJsonParam();
            return Json(param);
        }
        dynamic ReadJsonParam()
        {
            string json = (new StreamReader(Request.Body)).ReadToEnd();
            return JsonConvert.DeserializeObject(json);
        }
    }
}