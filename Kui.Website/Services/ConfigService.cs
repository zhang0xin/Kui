using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Resource.Config;
using Microsoft.Extensions.Configuration;

namespace Kui.Website.Services 
{
    public class ConfigService 
    {
        Dictionary<string, ResNode> _cache;
        ResourceConfig _resource;
        public ConfigService(IConfiguration config)
        {
            _cache = new Dictionary<string, ResNode>();
            _resource = new ResourceConfig();
            _resource.Root = config.GetSection("ResourceNodes").Get<ResNode>();
            InitCache(_resource.Root, "", "-");
        }
        void InitCache(ResNode node, string prefix, string seperator)
        {
            var childPath = node.Key;
            if (!string.IsNullOrEmpty(prefix))
                childPath = $"{prefix}{seperator}{childPath}";
            _cache.Add(childPath, node);

            if (node.Children == null) return;
            foreach(var childNode in node.Children)
            {
                InitCache(childNode, childPath, seperator);
            }
        }
        public ResNode GetNode(string path)
        {
            return _cache[path];
        }
        public ResNode[] GetNodes()
        {
            return _resource.Root.Children;
        }
    }
}