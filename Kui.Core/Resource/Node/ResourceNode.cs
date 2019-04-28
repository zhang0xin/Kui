using System;
using System.Collections.Generic;

namespace Kui.Core.Resource.Node
{
    public class ResourceNode
    {
        [Identity]
        public ulong  Key {get; set;}
        public DateTime CreateTime {get; set;}
        public DateTime UpdateTime {get; set;}
        
        public string Caption {get; set;}
        public string Description {get; set;}
        public string Path {get; set;}
    }
}