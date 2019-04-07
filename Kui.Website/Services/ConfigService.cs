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
        ResourceConfig _resource;
        public ConfigService(IConfiguration config)
        {
            _resource = new ResourceConfig();
            _resource.Root = config.GetSection("ResourceNodes").Get<ResNode>();
        }
        public ResNode[] GetSiteNodes()
        {
            return _resource.Root.Children;
        }
    }
}