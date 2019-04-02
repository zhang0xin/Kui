using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
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

        public void SaveSiteNode(BaseNode node)
        {
            var selectsql = "select * from base_node where key = @key";
            var insertsql = "insert into base_node (key, caption, description, path) values (@node); ";
            var updatesql = "update base_node set key=@key, caption=@caption, description=@description, path=@path); ";
            using(var conn = CreateConnection())
            {
                var queryNode = conn.QuerySingleOrDefault<BaseNode>(selectsql, node.Key);
                if(queryNode == null)
                {
                    conn.Execute(insertsql, node);
                }
                else
                {
                    conn.Execute(updatesql, node);
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
                    key         varchar(255)    PRIMARY KEY,
                    name        varchar(255)    NOT NULL,
                    value       varchar(255)    NOT NULL,
                    type        varchar(255)    NOT NULL
                );
            ";
            var connection = CreateConnection();
            connection.Execute(sql);
        }
    }
}