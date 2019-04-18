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
            dynamic model = CreateModelByNode(node); // todo : load datas 
            var editModel = new EditModel();
            editModel.Root.Name = node.Key;
            editModel.Root.Label = node.Text;
            PrepareElement(editModel.Root, model);
            return View(editModel);
        }
        void PrepareElement(Group group, object obj)
        {
            Type type = obj.GetType();

            var props = type.GetProperties();
            foreach(var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(TypeAttribute), true);
                if (attrs.Length == 0) continue;
                foreach(var attr in attrs) 
                {
                    var typeAttr = attr as TypeAttribute;
                    if (attr is GroupAttribute)
                    {
                        var subgroup = new Group(){Name = prop.Name, Label = typeAttr.Label};
                        PrepareElement(subgroup, prop.GetValue(obj));
                        group.AddChild(subgroup);
                    }
                    else if (attr is FieldAttribute)
                    {
                        var field = new Field{Name = prop.Name, Label = typeAttr.Label};
                        field.Value = prop.GetValue(obj) as string;
                        group.AddChild(field);
                    }
                }
            }
        }

    }
}