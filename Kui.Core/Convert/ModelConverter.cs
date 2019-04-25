using System.Collections.Generic;
using System.Linq;

namespace Kui.Core.Convert
{
    public class ModelConverter
    {
        public Element ConvertToElement(object model)
        {
            return ConvertToElement(null, model, new object[]{});
        }
        Element ConvertToElement(string propName, object propValue, object[] attrs)
        {
            var attr = attrs.FirstOrDefault() as TypeAttribute;
            if (attr == null) return null;

            if (attr is ListAttribute) 
                return ConvertToList(propName, propValue, attr, attrs[1] as TypeAttribute);
            else if (attr is GroupAttribute)
                return ConvertToGroup(propName, propValue, attr);
            else if (attr is FieldAttribute)
                return ConvertToField(propName, propValue, attr);

            return null;
        }
        Field ConvertToField(string propName, object propValue, TypeAttribute attr)
        {
            var field = new Field { Name = propName, Label = attr.Label };
            field.Value = propValue as string;
            return field;
        }
        Element ConvertToGroup(string propName, object propValue, TypeAttribute attr)
        {
            var group = new Group() { Name = propName, Label = attr.Label };
            var subprops = propValue.GetType().GetProperties();
            foreach (var subprop in subprops)
            {
                var subattrs = subprop.GetCustomAttributes(typeof(TypeAttribute), true);
                group.AddElement(ConvertToElement(subprop.Name, subprop.GetValue(propValue), subattrs));
            }
            return group;
        }
        List ConvertToList(string propName, object propValue, TypeAttribute listAttr, TypeAttribute itemAttr)
        {
            var list = new List()
            {
                Name = propName,
                Label = listAttr.Label
            };
            var itemType = propValue.GetType().GetGenericArguments()[0];
            var exampleItem = itemType.Assembly.CreateInstance(itemType.ToString());
            list.ExampleItem = ConvertToElement($"", exampleItem, new object[] { itemAttr });
            return list;
        }
    }
}