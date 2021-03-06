using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Resource.Node;

namespace Kui.Core.Resource.Persistence 
{
    public interface DataAccesser 
    {
        string PathSeparator {get; set;}
        IEnumerable<T> GetSiteNode<T>(string path) where T:ResourceNode;
        void SaveSiteNode(ResourceNode node);
        void RemoveSiteNode(string path);
    }
}