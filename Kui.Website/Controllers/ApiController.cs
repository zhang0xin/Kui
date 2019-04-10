using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kui.Core;
using Kui.Core.Resource.Node;
using Kui.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            return Json(_resourceService.Get<SiteNode>(path));
        }
        public IActionResult SaveSiteNode()
        {
            var param = ReadNodeFromJson(ReadJsonString());
            return Json(param);
        }
        dynamic ReadJsonString()
        {
            return (new StreamReader(Request.Body)).ReadToEnd();
        }
        dynamic ReadJsonParam()
        {
            return JsonConvert.DeserializeObject(ReadJsonString());
        }
        SiteNode ReadNodeFromJson(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            Type type = typeof(SiteNode).Assembly.GetType("Kui.Core.Resource.Node."+jsonObj.type.Value);
            JToken data = jsonObj.data;

            var instance = type.Assembly.CreateInstance(type.ToString());
            var porps = type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach(var prop in porps)
            {
                var propname = prop.Name.First().ToString().ToLower()+prop.Name.Substring(1);
                if (data[propname] == null) continue;
                prop.SetValue(instance, data[propname].Value<string>());
            }

            return (SiteNode) instance;
        }
    }
}