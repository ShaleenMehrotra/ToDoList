using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DataProvider
{
    [ExcludeFromCodeCoverage]
    public class DbProviderBase
    {
        protected readonly string _connectionString = "";

        public DbProviderBase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            };

            _connectionString = connectionStringBuilder.ConnectionString;
        }
    }
}