using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;
using Shared.ErrorModels;
using Store.Domain.Contracts;
using Store.Services;
using Store.Services.Abstractions;
using Store.Services.Mapping.Products;

namespace Store.web.Extention

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

            // Database context
            services.AddDbContext<Persistance.Data.Contexts.StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Dependency Injection setup
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IserviceManager, ServiceManager>();

            // AutoMapper
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));

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

