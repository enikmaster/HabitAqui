using HabitAqui.Data.Mocks;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;

namespace HabitAqui.Data.Seeders
{
    public static class HousingSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var housingService = serviceProvider.GetRequiredService<HabitacaoService>();
            var userManager = serviceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();
            var locadorService = serviceProvider.GetRequiredService<LocadorService>();
            var categoriaService = serviceProvider.GetRequiredService<CategoriaService>();
            try
            {
                var locador = await locadorService.GetLocadorByEmail("locador@locador.com");
                if(locador != null)
                {

                    // verify if the housing already exists ( to not add it)
                    var housingsList = await housingService.GetAllHabitacoesLocador(locador.Id);

                    if (!housingsList.Any())
                    {
                        var newCategory = CategoriaMock.GenerateCategoria();
                        await categoriaService.CreateCategoria(newCategory);

                        var newHousing = HousingMock.GenerateOneHabitacaoMock(locador.Id);
                        var newHousingCategoryRelation = HousingMock.GenerateHabitacaoCategoria(newHousing, newCategory);

                        newHousing.Categorias = new List<HabitacaoCategoria>() { newHousingCategoryRelation };

                        await housingService.CreateHabitacao(newHousing);
                    }
                }
            }
            catch(Exception ex)
            {
                // Console.WriteLine(ex.ToString());  
            }
        }
    }
}
