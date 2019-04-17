using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kui.Website.Models;
using Kui.Website.Services;
using Kui.Core.Resource.Node;
using Kui.Core.Resource.Config;

namespace Kui.Website.Controllers 
{
    public class BaseController : Controller
    {
        protected ConfigService _configService;
        public BaseController(ConfigService configService)
        {
            _configService = configService;
        }
        protected dynamic CreateModelByNode(ResNode node)
        {
            return this.GetType().Assembly.CreateInstance($"Kui.Website.Models.{node.Type}Model"); 
        }
        protected dynamic CreateModelByPath(string path)
        {
            var node = _configService.GetNode(path);
            return CreateModelByNode(node); 
        }
    }
}