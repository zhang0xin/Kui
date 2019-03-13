using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kui.Core.Node;
using Kui.Core.Persistence;
using Kui.Core.Persistence.Sqlite;

namespace Kui.Core
{
    public class KuiSite
    {
        static KuiSite _instance;
        public static KuiSite Singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KuiSite();
                }
                return _instance;
            }
        }

        DataAccesser _dataAccesser;
        public KuiSite()
        {
            _dataAccesser = new SqliteDataAccesser();
        }

        public IEnumerable<BaseNode> GetSubNodes(string path)
        {
            return GetSubNodes<BaseNode>(path);
        }
        public IEnumerable<T> GetSubNodes<T>(string path)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}