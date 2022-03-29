using DataProvider;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskDbProvider _taskDbProvider;

        public TaskController(ITaskDbProvider taskDbProvider)
        {
            _taskDbProvider = taskDbProvider;
        }

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var response = _taskDbProvider.GetTaskDetails(id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<int> Post(Task task)
        {
            var response = _taskDbProvider.StoreTaskDetails(task);
            return Ok();
        }
    }
}
