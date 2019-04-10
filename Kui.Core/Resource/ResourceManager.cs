using System;
using System.Collections.Generic;
using Kui.Core.Resource.Node;
using Kui.Core.Resource.Persistence;
using Kui.Core.Resource.Persistence.Sqlite;

namespace Kui.Core.Resource
{
    public class ResourceManager
    {
        static DataAccesser accessor = new SqliteDataAccesser("-");
        public static IEnumerable<T> Get<T>(string path) where T:SiteNode
        {
            return accessor.GetSiteNode<T>(path);
        }
        public static void Save<T>(T node) where T:SiteNode
        {
            accessor.SaveSiteNode(node);
        }
        public static void Remove(string path)
        {
            accessor.RemoveSiteNode(path);
        }
        public static IEnumerable<T> Query<T>()
        {
            throw new NotImplementedException();
        }
    }
    public class ResourceManager<T> where T:SiteNode
    {
        public IEnumerable<T> Get(string path)
        {
            return ResourceManager.Get<T>(path);
        }
        public void Save(T node)
        {
            ResourceManager.Save<T>(node);
        }
        public void Remove(string path)
        {
            ResourceManager.Remove(path);
        }
        public IEnumerable<T> Query()
        {
            return ResourceManager.Query<T>();
        }
    }
}