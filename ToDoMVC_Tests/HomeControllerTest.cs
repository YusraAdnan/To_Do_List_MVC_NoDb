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
            using var ctx = DbContextFactory.CreateUnique();
            ctx.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Task A", IsComplete = false });
            ctx.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Task B", IsComplete = true });
            ctx.SaveChanges();

            var controller = new HomeController(ctx);

            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as List<TaskItem>;
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.Count);
        }

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
