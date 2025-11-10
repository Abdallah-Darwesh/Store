using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;
using Persistance.Data.Contexts;
using Persistance.Identity.Context;
using Shared.ErrorModels;
using Store.Domain.Contracts;
using Store.Domain.Identity;
using Store.Services;
using Store.Services.Abstractions;
using Store.Services.Mapping.Products;
using Store.Services.Mapping.Orders; // ← إضافة OrderProfile namespace

namespace Store.Web.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Controllers
            services.AddControllers();

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Main App DbContext
            services.AddDbContext<StoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity DbContext
            services.AddDbContext<identityStoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            // Identity setup
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<identityStoreDbContext>()
            .AddDefaultTokenProviders();

            // Dependency Injection setup
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IserviceManager, ServiceManager>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new ProductProfile(configuration));
                cfg.AddProfile(new OrderProfile());
            });

            // Custom model validation error response
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(m => m.Value.Errors.Any())
                        .Select(m => new ValidationError
                        {
                            field = m.Key,
                            messages = m.Value.Errors.Select(e => e.ErrorMessage)
                        });

                    var response = new validationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
