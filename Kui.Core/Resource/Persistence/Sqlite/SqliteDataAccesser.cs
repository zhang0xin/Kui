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
using System.Text;

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
                var table = GetObjectsTableSql<T>();
                var sql = $"select * from ({table}) t where t.path = @path";
                object param = new {path};
                if (path.EndsWith(PathSeparator))
                {
                    sql = "select * from ({table}) t where t.path like @path1 and t.path not like @path2";
                    param = new{ path1=$"{path}_%", path2="{path}_%/%"};
                }
                sql += " order by update_time desc, create_time desc";

                return conn.Query<T>(sql, param);
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
                }
                SaveObject(node);
            }
        }
        public void RemoveSiteNode(string path)
        {
            using(var conn = CreateConnection())
            {
                var pathmode = $"{path}%";
                var keys = conn.Query<ulong>(
                    "select ObjectId from Objects where Property='Path' and Value like @path",
                    new { path = pathmode });
                conn.Execute( "delete from Objects where ObjectId in @keys", new{keys});
            }
        }
        public void RemoveSiteNode(ulong key)
        {
            DeleteObject(key);
        }

        public void DeleteObject(ulong id)
        {
            using(var conn = CreateConnection())
            {
                conn.Execute("delete from Objects where ObjectId = @id;", new{id});
            }
        }
        public void SaveObject(Object obj)
        {
            var type = obj.GetType();
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var idprop = props
                .Where(p => p.GetCustomAttribute(typeof(IdentityAttribute)) != null)
                .FirstOrDefault();
            using (var conn = CreateConnection())
            {
                ulong objectId = (ulong)idprop.GetValue(obj);
                var datas = conn.Query(
                    "select Property from Objects where ObjectId = @ObjectId", 
                    new {ObjectId = objectId});
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
                    if (prop == idprop) continue;
                    
                    var value = prop.GetValue(obj);
                    var param = new
                    {
                        Id = IdentityGenerator.NewGuid(),
                        ObjectId = objectId,
                        Property = prop.Name,
                        Value = value,
                        Type = value.GetType().ToString() 
                    };
                    if (dicActions[prop.Name] == "update")
                    {
                        conn.Execute(
                            "update Objects set Value=@Value, Type=@Type " +
                            "where ObjectId=@ObjectId and Property=@Property",
                            param
                        );
                    }
                    else if (dicActions[prop.Name] == "insert")
                    {
                        conn.Execute(
                            "insert into Objects (Id, ObjectId, Property, Value, Type) "+
                            "values(@Id, @ObjectId, @Name, @Value, @Type)",
                            param
                        );
                    }
                    else if (dicActions[prop.Name] == "delete")
                    {
                        conn.Execute(
                            "delete from Objects where ObjectId=@ObjectId and Property=@Property",
                            param);
                    }
                }
            }
        }
        string GetObjectsTableSql<T>()
        {
            Type type = typeof(T);
            var props = type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            var builder = new StringBuilder();
            foreach(var prop in props)
            {
                builder.Append($", group_concat(case Property when '{prop.Name}' then Value else NULL end");
            }
            return $" select ObjectId {builder} from Objects group by ObjectId ";
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
                create table Objects
                {
                    Id          integer     primary key,
                    ObjectId    integer     not null,
                    Property    text        not null,
                    Value       text,
                    Type        text
                };
            ";
            var connection = CreateConnection();
            connection.Execute(sql);
        }
    }
}