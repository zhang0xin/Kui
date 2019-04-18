using System.Collections.Generic;
using Kui.Core.Convert;

namespace Kui.Website.Models
{
    public class EditModel
    {
        public Group Root{get; set;} = new Group();
    }
    public abstract class Element
    {
        public string Name {get; set;}
        public string Label {get; set;}
        public Group Parent {get; set;}
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
    public class Group : Element
    {
        List<Element> _children = new List<Element>();
        public IEnumerable<Element> Children 
        {
            get
            {
                return _children;
            }
        }
        public void AddChild(Element ele)
        {
            _children.Add(ele);
            ele.Parent = this;
        }
    }
    public class Field : Element
    {
        public string Value {get; set;}
        public TypeAttribute Type {get; set;}
    }
}