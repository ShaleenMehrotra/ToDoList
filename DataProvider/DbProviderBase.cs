using Microsoft.Data.Sqlite;

namespace DataProvider
{
    public class DbProviderBase
    {
        protected readonly string _connectionString = "";

        public DbProviderBase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "../DataProvider/Database.db"
            };

            _connectionString = connectionStringBuilder.ConnectionString;
        }
    }
}