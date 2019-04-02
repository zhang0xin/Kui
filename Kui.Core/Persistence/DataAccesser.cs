using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Node;

namespace Kui.Core.Persistence 
{
    public interface DataAccesser 
    {
        void SaveSiteNode(BaseNode node);
    }
}