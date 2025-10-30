using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List_MVC_NoDb.Models;

namespace To_Do_List_MVC_NoDb.Controllers
{
    public class MicroApiController : Controller
    {
        private readonly ToDoDbContext _dbContext;
        public MicroApiController(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Endpoint (location address for other applications to query if they want to get all tasks )
        [HttpGet("api/tasks")] 
        public IActionResult GetAllTasks()
        {
            var tasks = _dbContext.TaskItems.ToList();
            return Json(tasks); //Returns json NOT HTML 
        }

        // GET: api/tasks/{id}   (get a single task by id)
        [HttpGet("api/tasks/{id}")]
        public IActionResult GetTask(Guid id)
        {
            var task = _dbContext.TaskItems.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            return Json(task);
        }

        // POST: api/tasks (create a new task)
        [HttpPost("api/tasks")]
        public IActionResult AddTaskAPI([FromBody] TaskItem newTask)
        {
            newTask.Id = Guid.NewGuid();
            _dbContext.TaskItems.Add(newTask);
            _dbContext.SaveChanges();

            return Ok(new { message = "Task created successfully", task = newTask });
        }

[HttpDelete("api/tasks/{id}")]
        public IActionResult DeleteTaskAPI(Guid id)
        {
            var task = _dbContext.TaskItems.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            _dbContext.TaskItems.Remove(task);
            _dbContext.SaveChanges();
            return Ok(new { message = "Task deleted successfully" });
        }        // DELETE: api/tasks/{id}
        
    }
}
