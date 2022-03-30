using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ToDoList
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateDatabase();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        // Function to create the database if it does not exist
        private static void CreateDatabase()
        {
            if (!File.Exists("../DataProvider/Database.db"))
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder
                {
                    DataSource = "../DataProvider/Database.db"
                };

                using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();

                    var createTableCommand = connection.CreateCommand();
                    createTableCommand.CommandText = "CREATE TABLE TODOLIST(id INTEGER PRIMARY KEY, description TEXT NOT NULL, created_date DATETIME NOT NULL);";
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
