using Microsoft.EntityFrameworkCore;
using Solvefy.Task.Data;

namespace Solvefy.Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<InventoryDbContext>(options =>
            {
                // options.UseSqlServer(builder.Configuration.GetConnectionString("ConSqlServer"));
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConPostgre"));

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
