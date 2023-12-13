using HabitAqui.Data.Mocks;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Data.Seeders
{
    public static class LandlordSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var locadorService = serviceProvider.GetRequiredService<LocadorService>();
            var userManager = serviceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();

            try
            {
                var locadorExiste = await locadorService.GetLocadorByEmail(LandlordMock.locadorEmail);

                if(locadorExiste == null) {
                    var newLocador = LandlordMock.GenerateLandlordMockExample();
                    await userManager.CreateAsync(newLocador);

                    await userManager.AddToRoleAsync(newLocador, Roles.Gestor.ToString());
                    await userManager.AddToRoleAsync(newLocador.Administradores.First(), Roles.Funcionario.ToString());
                }
            }
            catch (Exception ex)
            {

                // Console.WriteLine(ex.ToString());  
            }
        }

    }
}
