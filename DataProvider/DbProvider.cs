using Microsoft.Data.Sqlite;
using Models.Request;
using System;
using System.Collections.Generic;

namespace DataProvider
{
    public interface IDbProvider
    {
        int StoreTaskDetails(Task task);
        List<Task> RetrieveTaskDetails();
        int DeleteTaskDetails(int id);
    }

    public class DbProvider : DbProviderBase, IDbProvider
    {

        public DbProvider() : base()
        {

        }

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
                            CreatedDate = Convert.ToDateTime(reader.GetValue(2))
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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var updateTableCommand = connection.CreateCommand();
                    updateTableCommand.CommandText = $"INSERT INTO TODOLIST VALUES({task.Id}, \'{task.Description}\', \'{task.CreatedDate:yyyy-MM-dd HH:mm:ss}\');";

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
    }
}
