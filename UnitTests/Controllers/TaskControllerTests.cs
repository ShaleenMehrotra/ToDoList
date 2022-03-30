using DataProvider;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Request;
using Models.Response;
using Moq;
using System;
using System.Collections.Generic;
using ToDoList.Controllers;
using Xunit;

namespace UnitTests
{
    public class TaskControllerTests
    {
        private readonly Mock<IDbProvider> _dbProviderMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _dbProviderMock = new Mock<IDbProvider>();
            _taskController = new TaskController(_dbProviderMock.Object);

        }

        #region Get Calls Tests
        [Fact]
        public void ShouldReturnAllTasksWhenTaskIdExists()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task
                {
                    Id = 1,
                    Description = "Task 1",
                    CreatedDate = DateTime.Now
                },
                new Task
                {
                    Id = 2,
                    Description = "Task 2",
                    CreatedDate = DateTime.Now
                }
            };

            _dbProviderMock.Setup(db => db.RetrieveTaskDetails()).Returns(tasks);

            // Act
            var actionResult = _taskController.Get();

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(200, objectResult.StatusCode);
            var getTasksResponse = (GetTasksResponse)(objectResult).Value;
            Assert.Equal(2, getTasksResponse.TotalCount);
            Assert.Equal("Tasks found successfully", getTasksResponse.Result.Message);
            Assert.Equal(1, getTasksResponse.Tasks[0].Id);
            Assert.Equal("Task 1", getTasksResponse.Tasks[0].Description);
            Assert.Equal(2, getTasksResponse.Tasks[1].Id);
            Assert.Equal("Task 2", getTasksResponse.Tasks[1].Description);
        }

        [Fact]
        public void ShouldReturnEmptyListWhenTaskIdDoesNotExists()
        {
            // Arrange
            var tasks = new List<Task>();

            _dbProviderMock.Setup(db => db.RetrieveTaskDetails()).Returns(tasks);

            // Act
            var actionResult = _taskController.Get();

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(404, objectResult.StatusCode);
            var getTasksResponse = (GetTasksResponse)(objectResult).Value;
            Assert.Equal(0, getTasksResponse.TotalCount);
            Assert.Equal("No tasks found", getTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldCatchGetExceptionFromTheDatabase()
        {
            // Arrange
            var exception = new Exception("An exception occurred");

            _dbProviderMock.Setup(db => db.RetrieveTaskDetails()).Throws(exception);

            // Act
            var actionResult = _taskController.Get();

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
            var getTasksResponse = (GetTasksResponse)(objectResult).Value;
            Assert.Null(getTasksResponse.TotalCount);
            Assert.Equal("An exception occurred", getTasksResponse.Result.Message);
        }

        #endregion

        #region Post Calls Tests
        [Fact]
        public void ShouldCreateTaskWhenCorrectIdIsPassed()
        {
            // Arrange
            int id = 1;

            Task task = new Task
            {
                Id = id,
            };

            _dbProviderMock.Setup(db => db.StoreTaskDetails(It.IsAny<Task>())).Returns(id);

            // Act
            var actionResult = _taskController.Post(task);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(201, objectResult.StatusCode);
            var addTasksResponse = (AddTasksResponse)(objectResult).Value;
            Assert.Equal(1, addTasksResponse.Id);
            Assert.Equal("Task added successfully", addTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldNotCreateTaskWhenInvalidIdIsPassed()
        {
            // Arrange
            int id = -1;

            Task task = new Task
            {
                Id = id,
            };

            // Act
            var actionResult = _taskController.Post(task);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(400, objectResult.StatusCode);
            var addTasksResponse = (AddTasksResponse)(objectResult).Value;
            Assert.Equal(-1, addTasksResponse.Id);
            Assert.Equal("Task id cannot be less than or equal to 0", addTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldReturnNotFoundIfAffectedRowsIsZeroForPost()
        {
            // Arrange
            int id = 1;
            int affectedRows = 0;

            Task task = new Task
            {
                Id = id,
            };

            _dbProviderMock.Setup(db => db.StoreTaskDetails(It.IsAny<Task>())).Returns(affectedRows);

            // Act
            var actionResult = _taskController.Post(task);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(404, objectResult.StatusCode);
            var addTasksResponse = (AddTasksResponse)(objectResult).Value;
            Assert.Equal(0, addTasksResponse.Id);
            Assert.Equal("No task added", addTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldCatchPostExceptionFromTheDatabase()
        {
            // Arrange
            int id = 1;

            Task task = new Task
            {
                Id = id,
            };

            var exception = new Exception("An exception occurred");

            _dbProviderMock.Setup(db => db.StoreTaskDetails(It.IsAny<Task>())).Throws(exception);

            // Act
            var actionResult = _taskController.Post(task);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
            var addTasksResponse = (AddTasksResponse)(objectResult).Value;
            Assert.Equal("An exception occurred", addTasksResponse.Result.Message);
        }

        #endregion

        #region Delete Calls Tests
        [Fact]
        public void ShouldDeleteTaskWhenCorrectIdIsPassed()
        {
            // Arrange
            int id = 1;
            int affectedRows = 1;

            _dbProviderMock.Setup(db => db.DeleteTaskDetails(It.IsAny<int>())).Returns(affectedRows);

            // Act
            var actionResult = _taskController.Delete(id);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(200, objectResult.StatusCode);
            var deleteTasksResponse = (ApiResponseBase)(objectResult).Value;
            Assert.Equal("Task with id = 1 deleted successfully", deleteTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldNotDeleteTaskWhenInvalidIdIsPassed()
        {
            // Arrange
            int id = -1;
            int affectedRows = 1;

            _dbProviderMock.Setup(db => db.DeleteTaskDetails(It.IsAny<int>())).Returns(affectedRows);

            // Act
            var actionResult = _taskController.Delete(id);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(400, objectResult.StatusCode);
            var deleteTasksResponse = (ApiResponseBase)(objectResult).Value;
            Assert.Equal("Task id cannot be less than or equal to 0", deleteTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldReturnNotFoundIfAffectedRowsIsZeroForDelete()
        {
            // Arrange
            int id = 1;
            int affectedRows = 0;

            _dbProviderMock.Setup(db => db.DeleteTaskDetails(It.IsAny<int>())).Returns(affectedRows);

            // Act
            var actionResult = _taskController.Delete(id);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(404, objectResult.StatusCode);
            var deleteTasksResponse = (ApiResponseBase)(objectResult).Value;
            Assert.Equal("No task deleted", deleteTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldReturnTaskIdAsZero()
        {
            // Arrange
            int id = 1;

            Task task = new Task
            {
                Id = id,
            };

            _dbProviderMock.Setup(db => db.StoreTaskDetails(It.IsAny<Task>())).Returns(0);

            // Act
            var actionResult = _taskController.Post(task);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(404, objectResult.StatusCode);
            var addTasksResponse = (AddTasksResponse)(objectResult).Value;
            Assert.Equal(0, addTasksResponse.Id);
            Assert.Equal("No task added", addTasksResponse.Result.Message);
        }

        [Fact]
        public void ShouldCatchDeleteExceptionFromTheDatabase()
        {
            // Arrange
            int id = 1;

            var exception = new Exception("An exception occurred");

            _dbProviderMock.Setup(db => db.DeleteTaskDetails(It.IsAny<int>())).Throws(exception);

            // Act
            var actionResult = _taskController.Delete(id);

            // Assert
            Assert.NotNull(actionResult.Result);
            ObjectResult objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
            var deleteTasksResponse = (ApiResponseBase)(objectResult).Value;
            Assert.Equal("An exception occurred", deleteTasksResponse.Result.Message);
        }

        #endregion
    }
}
