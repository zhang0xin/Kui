using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core;
using Kui.Core.Node;

namespace Kui.Website.Services 
{
    public class KuiSiteService 
    {
        KuiSite _kuiSite;
        public KuiSiteService()
        {
            _kuiSite = KuiSite.Singleton;
        }
        public IEnumerable<SiteNode> GetSubNodes(string path)
        {
            return GetSubNodes<SiteNode>(path);
        }
        public IEnumerable<T> GetSubNodes<T>(string path)
        {
            return _kuiSite.GetSubNodes<T>(path);
        }
    }
}