using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using To_Do_List_MVC_NoDb;
using To_Do_List_MVC_NoDb.Models;

namespace ToDoMVC_Tests
{
    public static class DbContextFactory
    {
        public static ToDoDbContext CreateUnique()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            return new ToDoDbContext(options);
        }


    }
}
