using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_MVC_NoDb.Models;

namespace To_Do_List_MVC_NoDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

       private static List<TaskItem> tasks = new List<TaskItem>();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Show all the tasks
        public IActionResult Index()
        {
            //Make the connected view return the tasks list
            return View(tasks);
        }

        [HttpPost]
        public IActionResult AddTask(string title)
        {
            var task = new TaskItem { Id= Guid.NewGuid(), Title = title, IsComplete = false };
            tasks.Add(task);

            TempData["Success"] = "Task added successfully!";
            return RedirectToAction("Index");
        }
        // Toggle complete
        public IActionResult ToggleComplete(Guid id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
            }
            return RedirectToAction("Index");
        }
        
        // Delete task
        public IActionResult DeleteTask(Guid id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                TempData["Success"] = "Task deleted successfully!";
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
