using HabitAqui.Data;
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

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) //por agora não vamos usar contas confirmadas, depois alterar para true
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
            name: "default",
            pattern: "{controller=Habitacao}/{action=Index}/{id?}");
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
            {
                //Estamos a usar o await, logo temos que usar uma async task

                //checkar se a role já existe
                if (!await roleManager.RoleExistsAsync(role))
                {
                    //se não existir, vamos criar
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //Também podemos dar seed a user accounts 
        }
            using (var scope = app.Services.CreateScope())
            {

                var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                //verificar se existe
                string adminEmail = "admin@isec.pt";
                string password = "Test123!";

                if (await UserManager.FindByEmailAsync(adminEmail) == null)
                {
                    //criar uma conta
                    var user = new IdentityUser();
                    user.UserName = adminEmail;
                    user.Email = adminEmail;

                    await UserManager.CreateAsync(user, password);


                    await UserManager.AddToRoleAsync(user, "Administrador");
                }


                // Criação do user Funcionario
                string funcionarioEmail = "funcionario@isec.pt";
                string funcionarioPassword = "Test123!";
                if (await UserManager.FindByEmailAsync(funcionarioEmail) == null)
                {
                    var funcionarioUser = new IdentityUser { UserName = funcionarioEmail, Email = funcionarioEmail };
                    await UserManager.CreateAsync(funcionarioUser, funcionarioPassword);
                    await UserManager.AddToRoleAsync(funcionarioUser, "Funcionario");
                }


                // Criação do user Gestor
                string gestorEmail = "gestor@isec.pt";
                string gestorPassword = "Test123!";
                if (await UserManager.FindByEmailAsync(gestorEmail) == null)
                {
                    var gestorUser = new IdentityUser { UserName = gestorEmail, Email = gestorEmail };
                    await UserManager.CreateAsync(gestorUser, gestorPassword);
                    await UserManager.AddToRoleAsync(gestorUser, "Gestor");
                }

                // Criação do user Cliente
                string clienteEmail = "cliente@isec.pt";
                string clientePassword = "Test123!";
                if (await UserManager.FindByEmailAsync(clienteEmail) == null)
                {
                    var clienteUser = new IdentityUser { UserName = clienteEmail, Email = clienteEmail };
                    await UserManager.CreateAsync(clienteUser, clientePassword);
                    await UserManager.AddToRoleAsync(clienteUser, "Cliente");
                }
            }
            app.Run();

        }
    }

        
    
   
