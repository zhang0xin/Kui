using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core;
using Kui.Core.Resource;
using Kui.Core.Resource.Node;

namespace Kui.Website.Services 
{
    public class ResourceService 
    {
        public ResourceService()
        {
        }
        public IEnumerable<T> Get<T>(string path) where T:SiteNode
        {
            return ResourceManager.Get<T>(path);
        }
        public void Save<T>(T node) where T:SiteNode
        {
            ResourceManager.Save(node);
        }
        public void Remove(string path)
        {
            ResourceManager.Remove(path);
        }
    }
}