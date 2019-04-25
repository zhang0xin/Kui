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
        public string NewChildPath(string parentPath)
        {
            return ResourceManager.NewChildPath(parentPath);
        }
        public IEnumerable<T> Get<T>(string path) where T:ResourceNode
        {
            return ResourceManager.Get<T>(path);
        }
        public void Save<T>(T node) where T:ResourceNode
        {
            ResourceManager.Save(node);
        }
        public void Remove(string path)
        {
            ResourceManager.Remove(path);
        }
    }
}