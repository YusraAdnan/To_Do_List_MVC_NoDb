using Microsoft.EntityFrameworkCore.Internal;
using To_Do_List_MVC_NoDb.Models;
using To_Do_List_MVC_NoDb.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ToDoMVC_Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Return_All_Tasks()
        {
            //Create the instance of fake db context
            using var ctx = DbContextFactory.CreateUnique();

            //Add to the db's TaskItem table 2 objects with hardcoded data
            ctx.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Task A", IsComplete = false });
            ctx.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Task B", IsComplete = true });

            //Save the changes in the fake db
            ctx.SaveChanges();

            //Call the controller and pass it the fake context 
            var controller = new HomeController(ctx);

            //We want the result for the Index Action to be a ViewResult because the actual method is returning a View
            var result = controller.Index() as ViewResult;

            //We are checking that the controller is actually returning something and not crashing
            Assert.IsNotNull(result);

            /* The view is holding data in list format in the actual method 
            Therefore, the current dummy data will be converted to a list and we 
            will check if the returning view actually returning a list of data  */
            var model = result.Model as List<TaskItem>;
            Assert.IsNotNull(model);

            //Checks if the view has exactly 2 tasks (same number of dummy tasks you inserted above)
            Assert.AreEqual(2, model.Count);
        }

        /*We need to check if AddTask 
         1. Creates a task with the user entered task title
         2. Saves it to the DB 
         3. Redirects to the correct view 
        */
        [TestMethod]
        public void AddTask_AddsAllTasks()
        {
            using var ctx = DbContextFactory.CreateUnique();
            var controller = new HomeController(ctx);

            var result = controller.AddTask("New Task") as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, ctx.TaskItems.Count());

        }
        [TestMethod]
        public void ToggleComplete_Sets_IsComplete_True()
        {
            using var ctx = DbContextFactory.CreateUnique();
            var taskId = Guid.NewGuid();

            ctx.TaskItems.Add(new TaskItem { Id = taskId, Title = "do chores", IsComplete = false });
            ctx.SaveChanges();

            var controller = new HomeController(ctx);
            controller.ToggleComplete(taskId);

            var updatedTask = ctx.TaskItems.Single(t => t.Id == taskId);
            Assert.IsTrue(updatedTask.IsComplete);
        }
        [TestMethod]
        public void DeleteTask()
        {
            using var ctx = DbContextFactory.CreateUnique();
            var taskId = Guid.NewGuid();

            ctx.TaskItems.Add(new TaskItem { Id = taskId, Title = "Cook Dinner", IsComplete = false });
            ctx.SaveChanges();

            var controller = new HomeController(ctx);
            controller.DeleteTask(taskId);

            Assert.AreEqual(0, ctx.TaskItems.Count());
        }
    }
}
