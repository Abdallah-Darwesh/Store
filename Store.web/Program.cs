using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;
using Persistance.Reposatories;
using StackExchange.Redis;
using Store.Domain.Contracts;
using Store.Services.Mapping;
using Store.web.Extention;
using Store.web.MedelWare;
using AutoMapper;
using Store.Services.Mapping.Baskets; 


namespace Store.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add all custom application services
            builder.Services.AddApplicationServices(builder.Configuration);

            // Redis connection
            builder.Services.AddSingleton<IConnectionMultiplexer>(services =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetValue<string>("RedisSettings:ConnectionString");
                return ConnectionMultiplexer.Connect(connectionString);
            });
            builder.Services.AddAutoMapper(cfg => {
              
                cfg.AddProfile<BasketProfile>();
            });
            builder.Services.AddScoped<ICashReposatory, CashRepository>();

            // AutoMapper configuration

            // Basket repository
            builder.Services.AddScoped<IBasketReposatory, BasketReposatory>();

            // builder.Services.AddSingleton<IDbInitializer, DbInitializer>();

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
