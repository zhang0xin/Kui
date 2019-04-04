using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Node;
using Kui.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kui.Website.Controllers 
{
    public class ApiController : Controller
    {
        ConfigService _configService;
        public ApiController(ConfigService configService)
        {
            _configService = configService;
        }

        public IActionResult GetSiteNodeTree()
        {
            return Json(_configService.GetSiteNodes());
        }
        public IActionResult GetSiteNode(string path)
        {
            throw new NotImplementedException();
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