using Microsoft.AspNetCore.Mvc;
using Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        public TaskController()
        {

        }

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult<int> Post(Task task)
        {
            return Ok();
        }
    }
}
