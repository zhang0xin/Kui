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
            EditModel editModel = PrepareEditModel(model);
            return View(editModel);
        }
        EditModel PrepareEditModel(Object model)
        {
            var editModel = new EditModel();
            PrepareElement(editModel.Elements, model);
            return editModel;
        }
        void PrepareElement(IList<Element> items, object obj)
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
                        var group = new Group(){Label = typeAttr.Label};
                        PrepareElement(group.Children, prop.GetValue(obj));
                        items.Add(group);
                    }
                    else if (attr is FieldAttribute)
                    {
                        var field = new Field{Label = typeAttr.Label};
                        field.Value = prop.GetValue(obj) as string;
                        items.Add(field);
                    }
                }
            }
        }

    }
}