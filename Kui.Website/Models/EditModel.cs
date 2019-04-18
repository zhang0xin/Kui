using System.Collections.Generic;
using Kui.Core.Convert;

namespace Kui.Website.Models
{
    public class EditModel
    {
        public IList<Element> Elements{get; set;} = new List<Element>();
    }
    public abstract class Element
    {
        public string Label {get; set;}
    }
    public class Group : Element
    {
        public IList<Element> Children {get; set;} = new List<Element>();
    }
    public class Field : Element
    {
        public string Value {get; set;}
        public TypeAttribute Type {get; set;}
    }
}