using DataProvider;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IDbProvider _dbProvider;

        public TaskController(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var response = _dbProvider.RetrieveTaskDetails(id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<int> Post([FromBody] Task task)
        {
            var response = _dbProvider.StoreTaskDetails(task);
            return Ok(response);
        }

        [HttpDelete]
        public ActionResult<int> Delete(int id)
        {
            _dbProvider.DeleteTaskDetails(id);
            return Ok();
        }
    }
}
