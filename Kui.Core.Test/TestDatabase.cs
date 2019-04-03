using System;
using Xunit;
using Kui.Core.Persistence.Sqlite;
using Kui.Core.Node;
using System.IO;

namespace Kui.Core.Test
{
    public class TestDatabase
    {
        [Fact]
        public void TestSqlite()
        {
            var accessor = new SqliteDataAccesser();
            Assert.True(File.Exists(Constants.SqliteFileName));
            PageNode node = new PageNode()
            {
                Key = "key",
                Caption = "caption",
                Description = "description",
                Path = "path",
                Title = "title",
                Content = "content"
            };
            accessor.SaveSiteNode(node);
        }
    }
}
