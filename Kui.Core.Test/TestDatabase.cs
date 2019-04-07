using System;
using Xunit;
using Kui.Core.Resource.Persistence.Sqlite;
using Kui.Core.Resource.Node;
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
                Caption = "caption",
                Description = "description",
                Path = "/path",
                Title = "title",
                Content = "content"
            };
            accessor.SaveSiteNode(node);
            var node2 = new PageNode()
            {
                Caption = "caption2",
                Description = "description2",
                Path = "/path",
                Title = "title2",
                Content = "content2"
            };
            accessor.SaveSiteNode(node2);

            var nodes = accessor.GetSiteNode<PageNode>("/path");
            Assert.Equal(2, nodes.Count());
            Assert.Equal(node.Key, nodes.Last().Key);
            Assert.Equal(node.Content, nodes.Last().Content);
            Assert.Equal(node2.Key, nodes.First().Key);
            Assert.Equal(node2.Content, nodes.First().Content);
        }
    }
}
