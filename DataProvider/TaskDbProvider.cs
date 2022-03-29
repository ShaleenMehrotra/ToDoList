using Models;
using System;

namespace DataProvider
{
    public interface ITaskDbProvider
    {
        int StoreTaskDetails(Task task);
        Task GetTaskDetails(int id);
    }

    public class TaskDbProvider : ITaskDbProvider
    {
        public Task GetTaskDetails(int id)
        {
            throw new NotImplementedException();
        }

        public int StoreTaskDetails(Task task)
        {
            throw new NotImplementedException();
        }
    }
}
