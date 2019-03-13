using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

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
            ";
            var connection = CreateConnection();
            connection.Execute(sql);
        }
    }
}