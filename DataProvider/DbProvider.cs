using Microsoft.Data.Sqlite;
using Models;
using System;

namespace DataProvider
{
    public interface IDbProvider
    {
        int StoreTaskDetails(Task task);
        Task RetrieveTaskDetails(int id);
        void DeleteTaskDetails(int id);
    }

    public class DbProvider : DbProviderBase, IDbProvider
    {

        public DbProvider() : base()
        {

        }

        public Task RetrieveTaskDetails(int id)
        {
            var task = new Task();

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    var readTableCommand = connection.CreateCommand();
                    readTableCommand.CommandText = $"SELECT * FROM TODOLIST WHERE id = {id};";

                    using (var reader = readTableCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            task.Id = Convert.ToInt32(reader.GetValue(0));
                            task.Description = reader.GetValue(1).ToString();
                            task.CreatedDate = Convert.ToDateTime(reader.GetValue(2));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return task;
        }

        public int StoreTaskDetails(Task task)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var updateTableCommand = connection.CreateCommand();
                        updateTableCommand.CommandText = $"INSERT INTO TODOLIST VALUES({task.Id}, \'{task.Description}\', \'{task.CreatedDate:yyyy-MM-dd HH:mm:ss}\');";

                        updateTableCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return task.Id;
        }

        public void DeleteTaskDetails(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var updateTableCommand = connection.CreateCommand();
                updateTableCommand.CommandText = "CREATE TABLE TODOLIST(id INTEGER PRIMARY KEY, description TEXT NOT NULL, created_date DATETIME, last_updated_date DATETIME);";
                updateTableCommand.ExecuteNonQuery();
            }
        }
    }
}
