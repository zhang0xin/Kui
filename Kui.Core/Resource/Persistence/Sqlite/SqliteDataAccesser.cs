using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using Dapper;
using Kui.Core.Resource.Node;

namespace Kui.Core.Resource.Persistence.Sqlite 
{
    public class SqliteDataAccesser : DataAccesser 
    {
        public string PathSeparator {get; set;}
        public SqliteDataAccesser(string pathSeparator = "/")
        {
            PathSeparator = pathSeparator;
            if(!File.Exists(Constants.SqliteFileName))
            {
                CreateSchema();
            }
        }

        public IEnumerable<T> GetSiteNode<T>(string path) where T:ResourceNode
        {
            using (var conn = CreateConnection())
            {
                var sql = "select * from base_node where path = @path";
                object param = new {path};
                if (path.EndsWith(PathSeparator))
                {
                    sql = "select * from base_node where path like @path1 and path not like @path2";
                    param = new{ path1=$"{path}_%", path2="{path}_%/%"};
                }
                sql += " order by update_time desc, create_time desc";
                var nodes = conn.Query<T>( sql, param);
                var keys = nodes.Select(n=> n.Key);

                var dataProps = conn.Query(
                    "select * from node_props where base_node_key in @keys", new {keys});
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

        public void SaveSiteNode(ResourceNode node)
        {
            using(var conn = CreateConnection())
            {
                node.UpdateTime = DateTime.Now;
                if(node.Key == 0)
                {
                    node.Key = IdentityGenerator.NewGuid();
                    node.CreateTime = DateTime.Now;
                    conn.Execute(
                        "insert into base_node (key, create_time, caption, description, path) "+
                        "values (@key, @createTime, @caption, @description, @path)", node);
                }
                else
                {
                    conn.Execute(
                        "update base_node set create_time=@createTime, caption=@caption, "+
                        "description=@description, path=@path where key=@key", node);
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
        public void RemoveSiteNode(string path)
        {
            using(var conn = CreateConnection())
            {
                var pathmode = $"{path}%";
                var nodes = conn.Query<ResourceNode>(
                    "select * from base_node where path like @path " +
                    "order by update_time desc, create_time desc", 
                    new { path = pathmode });
                var keys = nodes.Select(n=> n.Key);
                conn.Execute(
                    "delete from base_node where path like @path;"+
                    "delete from node_props where base_node_key in @keys", 
                    new{path = pathmode, keys});
            }
        }
        public void RemoveSiteNode(ulong key)
        {
            using(var conn = CreateConnection())
            {
                conn.Execute("delete from base_node where key = @key;"+
                    "delete from node_props where base_node_key = @key;", new{key});
            }
        }

        SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection($"data source = {Constants.SqliteFileName}");
        }
        void CreateSchema()
        {
            var sql = @"
                create table base_node
                (
                    key             integer     primary key, 
                    create_time     text,
                    update_time     text,
        
                    path            text        not null,
                    caption         text,
                    description     text        
                );
                create table node_props
                (
                    key             integer     primary key,

                    base_node_key   integer     not null, 
                    name            text        not null,
                    value           text        not null,
                    type            text        not null
                );
            ";
            var connection = CreateConnection();
            connection.Execute(sql);
        }
    }
}