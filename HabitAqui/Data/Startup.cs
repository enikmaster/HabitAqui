using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Data;

public enum Roles
{
    Administrador,
    Gestor,
    Funcionario,
    Cliente
}

public static class Startup
{
    public static async Task CriaDadosIniciais(UserManager<DetalhesUtilizador> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context
    )
    {
        

        await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Gestor.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Funcionario.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));

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
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
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
            await userManager.AddToRoleAsync(user, Roles.Administrador.ToString());
        }

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
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
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
            await userManager.AddToRoleAsync(funcionarioUser, Roles.Funcionario.ToString());
        }

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
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
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
            await userManager.AddToRoleAsync(gestorUser, Roles.Gestor.ToString());
        }

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
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
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
            await userManager.AddToRoleAsync(clienteUser, Roles.Cliente.ToString());
        }
    }
}