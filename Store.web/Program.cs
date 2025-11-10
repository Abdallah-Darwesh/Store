using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Data.Contexts;
using Persistance.Identity.Context;
using Store.Domain.Contracts;
using Store.Domain.Identity;
using Store.Services;
using Store.Services.Abstractions;
using Store.Services.Mapping.Baskets;
using Store.Web.Extension;
using StackExchange.Redis;
using AutoMapper;
using Persistance.Reposatories;
using Store.web.MedelWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Store.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add all custom application services
            builder.Services.AddApplicationServices(builder.Configuration);

            // Register ServiceManager for DI
            builder.Services.AddScoped<IserviceManager, ServiceManager>();

            // Redis connection
            builder.Services.AddSingleton<IConnectionMultiplexer>(services =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetValue<string>("RedisSettings:ConnectionString");
                return ConnectionMultiplexer.Connect(connectionString);
            });

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorization();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BasketProfile>());

            // Scoped Repositories
            builder.Services.AddScoped<ICashReposatory, CashRepository>();
            builder.Services.AddScoped<IBasketReposatory, BasketReposatory>();

            // Add Controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            // Serve static files
            app.UseStaticFiles();

            // Initialize databases
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await initializer.InitializeAsync();
                await initializer.InitializeIdentityAsync();
            }

            // Middleware
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
