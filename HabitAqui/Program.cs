using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services
            .AddDefaultIdentity<
                DetalhesUtilizador>(options =>
                options.SignIn.RequireConfirmedAccount =
                    false) //por agora não vamos usar contas confirmadas, depois alterar para true
            .AddRoles<IdentityRole>() //para conseguirmos criar roles para o identity
            .AddEntityFrameworkStores<ApplicationDbContext>();


        builder.Services.AddScoped<LocadorService>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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


        // tudo isto deve estar no ficheiro Startup.cs
        //criar um socpe
        using (var scope = app.Services.CreateScope())
        {
            //Para aceder aos serviços que configuramos acima
            //seeding initial data

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //agora criarmos um array de roles

            var roles = new[] { "Anonimo", "Cliente", "Funcionario", "Gestor", "Administrador", "Webmaster" };

            //agora vamos criar adicionar as roles ao sistema
            foreach (var role in roles)
                //Estamos a usar o await, logo temos que usar uma async task
                //checkar se a role já existe
                if (!await roleManager.RoleExistsAsync(role))
                    //se não existir, vamos criar
                    await roleManager.CreateAsync(new IdentityRole(role));
            //Também podemos dar seed a user accounts 
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();

            //verificar se existe
            const string adminEmail = "admin@isec.pt";
            const string password = "Test123!";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new DetalhesUtilizador
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nome = "Admin",
                    Apelido = "Admin",
                    Nif = "123456789",
                    Localizacao = new Localizacao
                    {
                        Morada = "Morada",
                        CodigoPostal = "1234-123",
                        Cidade = "Cidade",
                        Pais = "País"
                    },
                    Active = true
                };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Administrador");
            }

            // Criação do user Funcionario
            const string funcionarioEmail = "funcionario@isec.pt";
            const string funcionarioPassword = "Test123!";
            if (await userManager.FindByEmailAsync(funcionarioEmail) == null)
            {
                var funcionarioUser = new DetalhesUtilizador
                {
                    UserName = funcionarioEmail,
                    Email = funcionarioEmail,
                    Nome = "Funcionário",
                    Apelido = "Funcionário",
                    Nif = "123456789",
                    Localizacao = new Localizacao
                    {
                        Morada = "Morada",
                        CodigoPostal = "1234-123",
                        Cidade = "Cidade",
                        Pais = "País"
                    },
                    Active = true
                };
                await userManager.CreateAsync(funcionarioUser, funcionarioPassword);
                await userManager.AddToRoleAsync(funcionarioUser, "Funcionario");
            }

            // Criação de um user Gestor
            const string gestorEmail = "gestor@isec.pt";
            const string gestorPassword = "Test123!";
            if (await userManager.FindByEmailAsync(gestorEmail) == null)
            {
                var gestorUser = new DetalhesUtilizador
                {
                    UserName = gestorEmail,
                    Email = gestorEmail,
                    Nome = "Gestor",
                    Apelido = "Gestor",
                    Nif = "123456789",
                    Localizacao = new Localizacao
                    {
                        Morada = "Morada",
                        CodigoPostal = "1234-123",
                        Cidade = "Cidade",
                        Pais = "País"
                    },
                    Active = true
                };
                await userManager.CreateAsync(gestorUser, gestorPassword);
                await userManager.AddToRoleAsync(gestorUser, "Gestor");
            }

            // Criação de um user Cliente
            const string clienteEmail = "cliente@isec.pt";
            const string clientePassword = "Test123!";
            if (await userManager.FindByEmailAsync(clienteEmail) == null)
            {
                var clienteUser = new DetalhesUtilizador
                {
                    UserName = clienteEmail,
                    Email = clienteEmail,
                    Nome = "Cliente",
                    Apelido = "Cliente",
                    Nif = "123456789",
                    Localizacao = new Localizacao
                    {
                        Morada = "Morada",
                        CodigoPostal = "1234-123",
                        Cidade = "Cidade",
                        Pais = "País"
                    },
                    Active = true
                };
                await userManager.CreateAsync(clienteUser, clientePassword);
                await userManager.AddToRoleAsync(clienteUser, "Cliente");
            }
        }

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