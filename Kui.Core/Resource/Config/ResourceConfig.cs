using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kui.Core.Resource.Config 
{
    public class ResourceConfig 
    {
        public ResNode Root {get; set;}
    }
    public class ResNode
    {
        public string Key {get; set;}
        public string Text {get; set;}
        public ResNode[] Children {get; set;}
    }
}