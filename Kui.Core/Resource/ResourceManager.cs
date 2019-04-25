using System;
using System.Collections.Generic;
using Kui.Core.Resource.Node;
using Kui.Core.Resource.Persistence;
using Kui.Core.Resource.Persistence.Sqlite;

namespace Kui.Core.Resource
{
    public class ResourceManager
    {
        public static DataAccesser accessor = new SqliteDataAccesser("-");

        public static string NewChildPath(string parentPath)
        {
            return $"{parentPath}{accessor.PathSeparator}{IdentityGenerator.NewGuid()}";
        }
        public static IEnumerable<T> Get<T>(string path) where T:ResourceNode
        {
            return accessor.GetSiteNode<T>(path);
        }
        public static void Save<T>(T node) where T:ResourceNode
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
    public class ResourceManager<T> where T:ResourceNode
    {
        public string NewChildPath(string parentPath)
        {
            return ResourceManager.NewChildPath(parentPath);
        }
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