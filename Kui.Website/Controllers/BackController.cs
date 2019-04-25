using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kui.Website.Models;
using Kui.Website.Services;
using Kui.Core.Resource.Node;
using Kui.Core.Convert;
using System.Reflection;

namespace Kui.Website.Controllers 
{
    public class BackController : BaseController
    {
        public BackController(ConfigService configService) : base(configService)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/Back/Edit/{path}")]
        public IActionResult Edit(string path)
        {
            var node = _configService.GetNode(path);
            dynamic model = CreateModelByNode(node); 
            var editModel = new EditModel()
            {
                Root = new ModelConverter().ConvertToElement(model)
            };
            return View(editModel);
        }
    }
}