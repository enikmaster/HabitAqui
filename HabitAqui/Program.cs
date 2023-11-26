using HabitAqui.Data;
using HabitAqui.Models;
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
        app.MapRazorPages();

        //criar um socpe
        using (var scope = app.Services.CreateScope())
        {
            //Para aceder aos serviços que configuramos acima
            //seeding initial data

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //agora criarmos um array de roles

            var roles = new[] { "Anonimo", "Cliente", "Funcionario", "Gestor", "Administrador" };

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
            var adminEmail = "admin@isec.pt";
            var password = "Test123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                //criar uma conta
                var user = new DetalhesUtilizador();
                user.UserName = adminEmail;
                user.Email = adminEmail;
                user.Nome = "BURRO";
                user.Apelido = "ESTUPIDO";
                user.Nif = "123456789";
                user.Localizacao = new Localizacao();
                user.Localizacao.Morada = "Rua do Burro";
                user.Localizacao.CodigoPostal = "1234-123";
                user.Localizacao.Cidade = "Burro";
                user.Localizacao.Pais = "Burrolandia";
                user.Active = true;

                await userManager.CreateAsync(user, password);


                await userManager.AddToRoleAsync(user, "Administrador");
            }


            // Criação do user Funcionario
            var funcionarioEmail = "funcionario@isec.pt";
            var funcionarioPassword = "Test123!";
            if (await userManager.FindByEmailAsync(funcionarioEmail) == null)
            {
                var funcionarioUser = new DetalhesUtilizador
                {
                    Active = true, Localizacao = new Localizacao(), Nome = "Antonio", Apelido = "Jose",
                    Nif = "123456789", UserName = funcionarioEmail, Email = funcionarioEmail
                };
                funcionarioUser.Localizacao.Morada = "Rua do Burro";
                funcionarioUser.Localizacao.CodigoPostal = "1234-123";
                funcionarioUser.Localizacao.Cidade = "Burro";
                funcionarioUser.Localizacao.Pais = "Burrolandia";

                await userManager.CreateAsync(funcionarioUser, funcionarioPassword);
                await userManager.AddToRoleAsync(funcionarioUser, "Funcionario");
            }


            // Criação do user Gestor
            var gestorEmail = "gestor@isec.pt";
            var gestorPassword = "Test123!";
            if (await userManager.FindByEmailAsync(gestorEmail) == null)
            {
                var gestorUser = new DetalhesUtilizador
                {
                    Active = true, Nome = "Carlos", Apelido = "Miguel", Nif = "123453789",
                    Localizacao = new Localizacao(), UserName = gestorEmail, Email = gestorEmail
                };
                gestorUser.Active = true;
                gestorUser.Localizacao.Morada = "Rua do Burro";
                gestorUser.Localizacao.CodigoPostal = "1234-123";
                gestorUser.Localizacao.Cidade = "Burro";
                gestorUser.Localizacao.Pais = "Burrolandia";

                await userManager.CreateAsync(gestorUser, gestorPassword);
                await userManager.AddToRoleAsync(gestorUser, "Gestor");
            }

            // Criação do user Cliente
            var clienteEmail = "cliente@isec.pt";
            var clientePassword = "Test123!";
            if (await userManager.FindByEmailAsync(clienteEmail) == null)
            {
                var clienteUser = new DetalhesUtilizador
                {
                    Nome = "AntoOOOnio", Apelido = "JosSe", Nif = "123456189", Localizacao = new Localizacao(),
                    UserName = clienteEmail, Email = clienteEmail
                };
                clienteUser.Active = true;
                clienteUser.Localizacao.Morada = "Rua do Burro";
                clienteUser.Localizacao.CodigoPostal = "1234-123";
                clienteUser.Localizacao.Cidade = "Burro";
                clienteUser.Localizacao.Pais = "Burrolandia";

                await userManager.CreateAsync(clienteUser, clientePassword);
                await userManager.AddToRoleAsync(clienteUser, "Cliente");
            }
        }

        app.Run();
    }
}