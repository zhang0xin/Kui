using System;
using Xunit;
using Kui.Core.Persistence.Sqlite;
using Kui.Core.Node;
using System.IO;
using System.Linq;

namespace Kui.Core.Test
{
    public class TestDatabase
    {
        [Fact]
        public void TestSqlite()
        {
            File.Delete(Constants.SqliteFileName);
            var accessor = new SqliteDataAccesser();
            Assert.True(File.Exists(Constants.SqliteFileName));

            var node = new PageNode()
            {
                Key = "key",
                Caption = "caption",
                Description = "description",
                Path = "/path",
                Title = "title",
                Content = "content"
            };
            accessor.SaveSiteNode(node);

            var node2 = accessor.GetSiteNode<PageNode>("/path").FirstOrDefault();
            Assert.Equal(node.Key, node2.Key);
            Assert.Equal(node.Content, node2.Content);
        }
    }
}
