using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_MVC_NoDb.Models;

namespace To_Do_List_MVC_NoDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoDbContext _dbContext;
        public HomeController(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Show all the tasks
        public IActionResult Index()
        {
            //Make the connected view return the tasks list
            var tasks = _dbContext.TaskItems.ToList();

            return View(tasks);
        }

        /*Action methods allows us to return different results (return Views, Redirect, etc) adding flexibility to controller methods
        represents what the controller sends back to the browser */
        [HttpPost]
        public IActionResult AddTask(string title)
        {
            var task = new TaskItem { Id= Guid.NewGuid(), Title = title, IsComplete = false };
            _dbContext.TaskItems.Add(task);
            _dbContext.SaveChanges();

            if (TempData != null)
            {
                //TempData is a way of storing data for one request/redirect
                TempData["Success"] = "Task added successfully!";
            }

            //reloads the task list now including the new task we do redirect to action when we dont want a new view to open
            return RedirectToAction("Index");
        }
        // Toggle complete
        public IActionResult ToggleComplete(Guid id)
        {
            var task = _dbContext.TaskItems.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
                _dbContext.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        
        // Delete task
        public IActionResult DeleteTask(Guid id)
        {
            var task = _dbContext.TaskItems.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _dbContext.TaskItems.Remove(task);
                _dbContext.SaveChanges();

                if (TempData != null)
                {
                    TempData["Success"] = "Task deleted successfully!";
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
