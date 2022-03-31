using DataProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IDbProvider _dbProvider;

        public TaskController(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetTasksResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseBase), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetTasksResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<GetTasksResponse> Get()
        {
            GetTasksResponse getTasksResponse = new GetTasksResponse
            {
                Result = new Result()
            };

            List<Task> tasks;

            try
            {
                tasks = _dbProvider.RetrieveTaskDetails();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                getTasksResponse.Result.Message = exception.Message;
                return StatusCode(500, getTasksResponse);
            }

            getTasksResponse.Tasks = tasks;
            getTasksResponse.TotalCount = tasks == null || !tasks.Any() ? 0 : tasks.Count;
            getTasksResponse.Result.Message = tasks == null || !tasks.Any() ? "No tasks found" : "Tasks found successfully";

            if (tasks == null || !tasks.Any())
            {
                return NotFound(getTasksResponse);
            }

            return Ok(getTasksResponse);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddTasksResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseBase), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AddTasksResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<AddTasksResponse> Post([FromBody] Task task)
        {
            AddTasksResponse addTasksResponse = new AddTasksResponse
            {
                Result = new Result()
            };

            if (task.Id <= 0)
            {
                addTasksResponse.Id = task.Id;
                addTasksResponse.Result.Message = "Task id cannot be less than or equal to 0";
                return BadRequest(addTasksResponse);
            }

            int taskId;

            try
            {
                // Adding this so that default date and time is not entered in the database
                if(task.LastUpdatedDate == DateTime.MinValue)
                {
                    task.LastUpdatedDate = DateTime.Now;
                }

                taskId = _dbProvider.StoreTaskDetails(task);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                addTasksResponse.Result.Message = exception.Message;
                return StatusCode(500, addTasksResponse);
            }

            addTasksResponse.Id = taskId;
            addTasksResponse.Result.Message = taskId == 0 ? "No task added" : "Task added successfully";

            if (taskId == 0)
            {
                return NotFound(addTasksResponse);
            }

            return StatusCode(201, addTasksResponse);
        }

        [HttpDelete("id")]
        [ProducesResponseType(typeof(ApiResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseBase), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponseBase), StatusCodes.Status500InternalServerError)]
        public ActionResult<ApiResponseBase> Delete(int id)
        {
            ApiResponseBase apiResponseBase = new ApiResponseBase
            {
                Result = new Result()
            };

            if (id <= 0)
            {
                apiResponseBase.Result.Message = "Task id cannot be less than or equal to 0";
                return BadRequest(apiResponseBase);
            }

            int rowsAffected;

            try
            {
                rowsAffected = _dbProvider.DeleteTaskDetails(id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                apiResponseBase.Result.Message = exception.Message;
                return StatusCode(500, apiResponseBase);
            }

            apiResponseBase.Result.Message = rowsAffected == 0 ? "No task deleted" : $"Task with id = {id} deleted successfully";

            if (rowsAffected == 0)
            {
                return NotFound(apiResponseBase);
            }

            return Ok(apiResponseBase);
        }
    }
}
