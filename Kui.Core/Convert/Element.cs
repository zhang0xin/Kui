using System.Collections.Generic;

namespace Kui.Core.Convert
{
    public abstract class Element
    {
        public string Name {get; set;}
        public string Label {get; set;}
        public Element Parent {get; set;}
        public string Path 
        {
            get 
            {
                Element curr = this;
                string path = curr.Name; 
                while(curr.Parent != null)
                {
                    path = $"{curr.Parent.Name}-{path}";
                    curr = curr.Parent;
                }
                return path;
            }
        } 
    }
    public class List : Element 
    {
        public Element ExampleItem {get; set;}
    }
    public class Group : Element
    {
        protected List<Element> _elements = new List<Element>();
        public IEnumerable<Element> Elements 
        {
            get
            {
                return _elements;
            }
        }
        public void AddElement(Element ele)
        {
            _elements.Add(ele);
            ele.Parent = this;
        }
    }
    public class Field : Element
    {
        public string Value {get; set;}
        public TypeAttribute Type {get; set;}
    }
}