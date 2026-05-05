using Booking.Service.Abstracts;
using Booking.Service.Implementation;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Service.Abstracts;
using Restaurant.Service.Implementation;

namespace Restaurant.Service
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddSeervicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IRestaurantService), typeof(RestaurantService));
            services.AddScoped(typeof(ITableService), typeof(TableService));
            services.AddScoped(typeof(ITokernService), typeof(TokenService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IEmailSender), typeof(EmailSender));
            services.AddScoped(typeof(IInvetationAdminSender), typeof(InvetationAdminSender));
            services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));
            services.AddScoped(typeof(IBookService), typeof(BookService));
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddHttpContextAccessor();


            services.AddHangfire(x =>
               x.UseSqlServerStorage(configuration.GetConnectionString("IdentityConnection")));

            services.AddHangfireServer();
            services.AddSignalR();

            return services;
        }
    }
}
