using Booking.Service.Abstracts;
using Booking.Service.Base;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Restaurant.Cor;
using Restaurant.Cor.MiddleWare;
using Restaurant.Data.Entities.Identity;
using Restaurant.Infra;
using Restaurant.Infra.Data;
using Restaurant.Infra.Identity;
using Restaurant.Infra.Identity.Seeding;
using Restaurant.Service;
using Restaurant.Service.Abstracts;
using System.Globalization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfractureDependencies(builder.Configuration);
builder.Services.AddSeervicesDependencies(builder.Configuration);
builder.Services.AddCoreDependencies();

#region Localization
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(opt =>
    {
        opt.ResourcesPath = "Resources";
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        List<CultureInfo> supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG"),
            new CultureInfo("fr-FR"),
            new CultureInfo("en-GB")
        };

        options.DefaultRequestCulture = new RequestCulture("en-GB");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });


#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var sccope = app.Services.CreateScope())
{
    var recurringJobManager = sccope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<IRestaurantService>(
        "update-ratings",
        x => x.UpdateAllRestaurants(),
        Cron.Minutely);

    recurringJobManager.AddOrUpdate<IBookService>(
    "auto-cancel-reservations",
    x => x.AutoCancel(),
    Cron.Minutely);

    recurringJobManager.AddOrUpdate<IBookService>(
    "auto-compelete-reservations",
    x => x.AutoCompelete(),
    Cron.Minutely);
    recurringJobManager.AddOrUpdate<IBookService>(
    "reservation-reminders",
    x => x.RememberUser(),
    Cron.Minutely);

    #region auto migration

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbcontext = services.GetRequiredService<AppDbContext>();
    var dbcontextIdentity = services.GetRequiredService<IdentityAppDbContext>();

    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        await dbcontext.Database.MigrateAsync();
        await dbcontextIdentity.Database.MigrateAsync();

        var usermanager = services.GetRequiredService<UserManager<AppUser>>();
        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedingRole.GetRole(rolemanager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "an error has been occur during apply migration");
    }
    finally { scope.Dispose(); }
    #endregion


    //Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseHttpsRedirection();
    app.UseCors("AllowFrontend");
    app.UseHangfireDashboard("/hangfire");

    app.UseAuthentication();   // الأول
    app.UseAuthorization();    // بعده


    app.MapControllers();
    app.MapGet("/", () => "API is running 🚀");
    app.MapHub<ReservationHub>("/ReservationHub");


    app.Run();
}
