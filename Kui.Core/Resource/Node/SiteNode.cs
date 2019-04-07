using System;

namespace Kui.Core.Resource.Node
{
    public class SiteNode
    {
        public ulong  Key {get; set;}
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
        
        public string Caption {get; set;}
        public string Description {get; set;}
        public string Path {get; set;}
    }
}