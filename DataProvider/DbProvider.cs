using Microsoft.Data.Sqlite;
using Models.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DataProvider
{
    public interface IDbProvider
    {
        int StoreTaskDetails(Task task);
        List<Task> RetrieveTaskDetails();
        int DeleteTaskDetails(int id);
    }

    [ExcludeFromCodeCoverage]
    public class DbProvider : DbProviderBase, IDbProvider
    {
        public DbProvider() : base() { }

        public List<Task> RetrieveTaskDetails()
        {
            var tasks = new List<Task>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var readTableCommand = connection.CreateCommand();
                readTableCommand.CommandText = $"SELECT * FROM TODOLIST;";

                using (var reader = readTableCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Task
                        {
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            Description = reader.GetValue(1).ToString(),
                            LastUpdatedDate = Convert.ToDateTime(reader.GetValue(2))
                        };

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public int StoreTaskDetails(Task task)
        {
            int rowsAffected = 0;

            string insertQuery = $"INSERT INTO TODOLIST VALUES({task.Id}, \'{task.Description}\', \'{task.LastUpdatedDate:yyyy-MM-dd HH:mm:ss}\');";
            string updateQuery = $"UPDATE TODOLIST SET description = \'{task.Description}\', last_updated_date = \'{task.LastUpdatedDate:yyyy-MM-dd HH:mm:ss}\' WHERE id = {task.Id};";

            // If task id already exists, update the description
            string commandText = TaskIdExists(task.Id) ? updateQuery : insertQuery;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var updateTableCommand = connection.CreateCommand();
                    updateTableCommand.CommandText = commandText;

                    rowsAffected = updateTableCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }

            if (rowsAffected == 0)
            {
                return 0;
            }

            return task.Id;
        }

        public int DeleteTaskDetails(int id)
        {
            int rowsAffected = 0;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var updateTableCommand = connection.CreateCommand();
                    updateTableCommand.CommandText = $"DELETE FROM TODOLIST WHERE id = {id};";

                    rowsAffected = updateTableCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }

            if (rowsAffected == 0)
            {
                return 0;
            }

            return rowsAffected;
        }

        private bool TaskIdExists(int id)
        {
            bool existsId = false;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var readTableCommand = connection.CreateCommand();
                readTableCommand.CommandText = $"SELECT * FROM TODOLIST WHERE id = {id};";

                using (var reader = readTableCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existsId = true;
                    }
                }
            }

            return existsId;
        }
    }
}
