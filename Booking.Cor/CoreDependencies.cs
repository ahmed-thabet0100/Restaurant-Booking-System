using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Cor.Behavior;
using Restaurant.Service;
using System.Reflection;


namespace Restaurant.Cor
{
    public static class CoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddScoped<ResponseHandler>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // 🔥 Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // 
            // 🔥 Register Validation Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;

        }
    }
}
