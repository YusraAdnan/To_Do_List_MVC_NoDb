using Microsoft.EntityFrameworkCore;
using To_Do_List_MVC_NoDb.Models;
using Microsoft.OpenApi.Models;
namespace To_Do_List_MVC_NoDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "To-Do List MVC Micro API",
                    Version = "v1",
                    Description = "This Swagger UI lets you test the To-Do List API routes built into your MVC project."
                });
            });
            builder.Services.AddDbContext<ToDoDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("LeaveManagementDb")));
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "To-Do API v1");
                c.RoutePrefix = "swagger"; // or "" if you want Swagger at root
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
