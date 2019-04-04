using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using Dapper;
using Kui.Core.Node;

namespace Kui.Core.Persistence.Sqlite 
{
    public class SqliteDataAccesser : DataAccesser 
    {
        public SqliteDataAccesser()
        {
            if(!File.Exists(Constants.SqliteFileName))
            {
                CreateSchema();
            }
        }

        public IEnumerable<T> GetSiteNode<T>(string path) where T:SiteNode
        {
            using (var conn = CreateConnection())
            {
                var nodes = conn.Query<T>(
                    "select * from base_node where path = @path", new{ path });
                var keys = nodes.Select(n=> n.Key);
                var dataProps = conn.Query(
                    "select * from node_props where base_node_key in (@keys)", new {keys});
                var dicProps = new Dictionary<string, object>();
                foreach(var prop in dataProps)
                {
                    dicProps.Add($"{prop.base_node_key}:{prop.name}", prop.value);
                }
                foreach(var node in nodes)
                {
                    var type = node.GetType();
                    var props = type.GetProperties(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    foreach(var prop in props)
                    {
                        prop.SetValue(node, dicProps[$"{node.Key}:{prop.Name}"]);
                    }
                }
                return nodes;
            }
        }

        public void SaveSiteNode(SiteNode node)
        {
            using(var conn = CreateConnection())
            {
                var queryNode = conn.QuerySingleOrDefault<SiteNode>(
                    "select * from base_node where key = @key", node );
                if(queryNode == null)
                {
                    conn.Execute(
                        "insert into base_node (key, caption, description, path) "+
                        "values (@key, @caption, @description, @path)", node);
                }
                else
                {
                    conn.Execute(
                        "update base_node set key=@key, caption=@caption, "+
                        "description=@description, path=@path ", node);
                }

                var props = node.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                var datas = conn.Query(
                    "select name from node_props where base_node_key = @baseNodeKey", 
                    new {baseNodeKey = node.Key});
                var dicActions = new Dictionary<string, string>();

                foreach(var data in datas)
                {
                    dicActions[data.name] = "delete";
                }
                foreach(var prop in props)
                {
                    if(dicActions.ContainsKey(prop.Name)) dicActions[prop.Name] = "update";
                    else dicActions[prop.Name] = "insert";
                }
                foreach(var prop in props)
                {
                    if (dicActions[prop.Name] == "update")
                    {
                        var value = prop.GetValue(node);
                        conn.Execute(
                            "update node_props set value=@value, type=@type " +
                            "where base_node_key=@baseNodeKey and name=@name",
                            new
                            {
                                baseNodeKey = node.Key,
                                name = prop.Name,
                                value = value,
                                type = value.GetType().ToString() 
                            }
                        );
                    }
                    else if (dicActions[prop.Name] == "insert")
                    {
                        var value = prop.GetValue(node);
                        conn.Execute(
                            "insert into node_props (key, base_node_key, name, value, type) "+
                            "values(@key, @baseNodeKey, @name, @value, @type)",
                            new
                            {
                                key = IdentityGenerator.NewGuid(),
                                baseNodeKey = node.Key,
                                name = prop.Name,
                                value = value,
                                type = value.GetType().ToString() 
                            }
                        );
                    }
                    else if (dicActions[prop.Name] == "delete")
                    {
                        conn.Execute("delete from node_props where base_node_key=@baseNodeKey and name=@name",
                            new { baseNodeKey = node.Key, name = prop.Name });
                    }
                }
            }
        }

        SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection($"data source = {Constants.SqliteFileName}");
        }
        void CreateSchema()
        {
            var sql = @"
                CREATE TABLE base_node
                (
                    key         varchar(255)    PRIMARY KEY,
                    path        varchar(255)    NOT NULL,
                    caption     varchar(255)    NOT NULL,
                    description varchar(255)
                );
                CREATE TABLE node_props
                (
                    key             INTEGER         PRIMARY KEY,
                    base_node_key   varchar(255)    NOT NULL, 
                    name            varchar(255)    NOT NULL,
                    value           varchar(255)    NOT NULL,
                    type            varchar(255)    NOT NULL
                );
            ";
            var connection = CreateConnection();
            connection.Execute(sql);
        }
    }
}