using Booking.Infra.Repostries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Data.Entities.Identity;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;
using Restaurant.Infra.Identity;
using Restaurant.Infra.InfraBases;
using Restaurant.Infra.Repostries;
using System.Text;
using Talabat.Core.Repo.Contarct;
using Talabat.Repo.Repo_Impelemnt;

namespace Restaurant.Infra
{
    public static class InfraDependencies
    {
        public static IServiceCollection AddInfractureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Db for App
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Add Db for Identity
            services.AddDbContext<IdentityAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityAppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:ValidAudience"],

                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:ValidIssuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/ReservationHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();



            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRestaurantRepo), typeof(RestaurantRepo));
            services.AddScoped(typeof(ITableRepo), typeof(TableRepo));
            services.AddScoped(typeof(IInvetationRepo), typeof(InvetationRepo));
            services.AddScoped(typeof(IReservationRepo), typeof(ReservationRepo));
            return services;
        }
    }

}
