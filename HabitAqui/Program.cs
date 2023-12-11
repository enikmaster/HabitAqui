using HabitAqui.Data;
using HabitAqui.Data.Seeders;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        /*builder.Services.AddIdentity<DetalhesUtilizador, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();*/
        builder.Services
            .AddDefaultIdentity<DetalhesUtilizador>(options =>
                options.SignIn.RequireConfirmedAccount =
                    false) //por agora não vamos usar contas confirmadas, depois alterar para true
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        // Adiciona os Services de cada controller
        builder.Services.AddScoped<LocadorService>();
        builder.Services.AddScoped<HabitacaoService>();
        builder.Services.AddScoped<CategoriaService>();

        builder.Services.AddControllersWithViews();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
            /*// Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;*/
        });
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.Database.ExecuteSqlRaw("SELECT 1");

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await Startup.CriaDadosIniciais(userManager, roleManager);

                // ==== Database seeding ====
                var serviceScope = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
                // call the seeders
                LandlordSeeder.InitializeAsync(scope.ServiceProvider).Wait();
                HousingSeeder.InitializeAsync(scope.ServiceProvider).Wait();
                // ==== END OF THE DATA SEEDING ====

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            "default",
            "{controller=Habitacao}/{action=Index}/{id?}");
        app.MapRazorPages();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "DetalhesLocador",
                "Locador/Detalhes/{id}",
                new { controller = "Locador", action = "Detalhes" });
            endpoints.MapControllerRoute(
                "AtualizarLocador",
                "Locador/Update/{id}",
                new { controller = "Locador", action = "Update" });
            endpoints.MapControllerRoute(
                "ApagarLocador",
                "Locador/Delete/{id}",
                new { controller = "Locador", action = "Delete" });
            endpoints.MapRazorPages();
        });
        app.Run();
    }
}