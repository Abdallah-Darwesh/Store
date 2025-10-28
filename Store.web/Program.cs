using Microsoft.AspNetCore.Mvc;
using Persistance;
using Store.Domain.Contracts;
using Store.web.Extention;
using Store.web.MedelWare;

namespace Store.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add all custom application services
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Serve static files
            app.UseStaticFiles();

            // Initialize database
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.InitializeAsync();
            }

            // Global error handling middleware
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
