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
            dynamic model = CreateModelByNode(node); // todo : load datas 
            var editModel = new EditModel();
            editModel.Root.Name = node.Key;
            editModel.Root.Label = node.Text;
            PrepareElement(editModel.Root, model);
            return View(editModel);
        }
        Element ConvertToElementTree(string propName, object propValue, object[] attrs)
        {
            var attr = attrs.FirstOrDefault();
            if (attr == null) return null;

            var typeAttr = attr as TypeAttribute;
            if (attr is ListAttribute)
            {
                var itemAttr = attrs[1];
                var list = new List() { 
                    Name = propName, Label = typeAttr.Label 
                };
                var items = propValue as IEnumerable<object>;
                var idx = 0;
                foreach(var item in items)
                {
                    list.AddElement(ConvertToElementTree($"#{idx}", item, new object[]{itemAttr}));
                    idx++;
                }
                var itemType = items.GetType().GetGenericArguments()[0];
                var exampleItem = itemType.Assembly.CreateInstance(itemType.ToString());
                list.ExampleItem = ConvertToElementTree($"", exampleItem, new object[]{itemAttr});
                return list;
            }
            else if (attr is GroupAttribute)
            {
                var group = new Group() { Name = propName, Label = typeAttr.Label };
                var subprops = propValue.GetType().GetProperties();
                foreach(var subprop in subprops)
                {
                    var subattrs = subprop.GetCustomAttributes(typeof(TypeAttribute), true);
                    group.AddElement(ConvertToElementTree(subprop.Name, subprop.GetValue(propValue), subattrs));
                }
                return group;
            }
            else if (attr is FieldAttribute)
            {
                var field = new Field { Name = propName, Label = typeAttr.Label };
                field.Value = propValue as string;
                return field;
            }
            return null;
        }
        void PrepareElement(Combination comb, object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(TypeAttribute), true);
                var attr = attrs.FirstOrDefault();
                if (attr == null) continue;

                var typeAttr = attr as TypeAttribute;
                if (attr is GroupAttribute)
                {
                    var subgroup = new Group() { Name = prop.Name, Label = typeAttr.Label };
                    PrepareElement(subgroup, prop.GetValue(obj));
                    comb.AddElement(subgroup);
                }
                if (attr is ListAttribute)
                {
                    var sublist = new List() { Name = prop.Name, Label = typeAttr.Label };
                    PrepareElement(sublist, prop.GetValue(obj));
                    comb.AddElement(sublist);
                }
                else if (attr is FieldAttribute)
                {
                    var field = new Field { Name = prop.Name, Label = typeAttr.Label };
                    field.Value = prop.GetValue(obj) as string;
                    comb.AddElement(field);
                }
            }

            // if comb is list do this
            var items = obj as IEnumerable<object>;
            foreach(var item in items)
            {
                /* 
                //object has attr prop
                if(group)
                {
                }
                // simple type
                else if (field)
                {
                }
                */
            }
        }
    }
}