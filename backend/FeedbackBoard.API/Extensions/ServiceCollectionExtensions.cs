using FeedbackBoard.Application.Interfaces;
using FeedbackBoard.Application.Mappings;
using FeedbackBoard.Application.Services;
using FeedbackBoard.Domain.Interfaces;
using FeedbackBoard.Infrastructure.Data;
using FeedbackBoard.Infrastructure.Repositories;
using FeedbackBoard.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FeedbackBoard.API.Extensions
{
    /// <summary>
    /// Extension methods for service registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application services to the DI container
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(FeedbackMappingProfile));

            // Add application services
            services.AddScoped<IFeedbackService, FeedbackService>();

            return services;
        }

        /// <summary>
        /// Adds infrastructure services to the DI container
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Use in-memory database for development/testing
                services.AddDbContext<FeedbackBoardDbContext>(options =>
                    options.UseInMemoryDatabase("FeedbackBoardDb"));
            }
            else
            {
                services.AddDbContext<FeedbackBoardDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            // Add repositories and unit of work
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Adds CORS policy for frontend
        /// </summary>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    var frontendUrl = configuration["Frontend:Url"] ?? "http://localhost:3000";
                    
                    builder
                        .WithOrigins(frontendUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
