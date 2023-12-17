using HabitAqui.Data.Mocks;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;

public static class LandlordSeeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        const string password = "Test123!"; // Use a secure password in production

        try
        {
            // Ensure the roles exist
            string[] roles = new string[] { "Gestor", "Administrador" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Step 1: Create GestorPrincipal users
            var gestores = LandlordMock.GenerateGestorPrincipalMocks();
            foreach (var gestor in gestores)
            {
                var user = await userManager.FindByEmailAsync(gestor.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(gestor, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(gestor, "Gestor");
                    }
                    else
                    {
                        // Log errors and handle user creation failure
                        continue;
                    }
                }
            }
            // Step 1.5: Create Funcionario users
            var funcionarios = LandlordMock.GenerateFuncionarioMocks();
            foreach (var funcionario in funcionarios)
            {
                var user = await userManager.FindByEmailAsync(funcionario.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(funcionario, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(funcionario, "Funcionario");
                    }
                    else
                    {
                        // Log errors and handle user creation failure
                        continue;
                    }
                }
            }

            // Step 2: Create Locador instances and associate them with gestores
            var landlords = LandlordMock.GenerateLandlordMocks(gestores,funcionarios);
            foreach (var landlord in landlords)
            {
                var aux = 0;
                var user = await userManager.FindByEmailAsync(landlord.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(landlord, password);
                    if (result.Succeeded)
                    {
                        // If landlords have a specific role, assign it here
                        if(aux > 2)
                        {
                            await userManager.AddToRoleAsync(landlord, "Cliente");
                        }
                        else await userManager.AddToRoleAsync(landlord, "Gestor");
                        aux++;
                    }
                    else
                    {
                        // Log errors and handle landlord creation failure
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception
        }
    }
}
