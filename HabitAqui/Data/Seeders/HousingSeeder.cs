using HabitAqui.Data.Mocks;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Cryptography.Xml;

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

            var reservaService = serviceProvider.GetRequiredService<ReservasService>();

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var locadores = new List<Locador>
            {
                await locadorService.GetLocadorByEmail("remax@isec.pt"),
                await locadorService.GetLocadorByEmail("era@isec.pt"),
                await locadorService.GetLocadorByEmail("kw@isec.pt")
            };

                if (locadores != null)
                {

                    // verify if the housing already exists ( to not add it)
                    var housingsList = await housingService.GetAllHabitacoesLocador("NULL");

                    if (!housingsList.Any())
                    {
                        var newCategories = CategoriaMock.GenerateListCategorias();
                        foreach (var categoria in newCategories)
                        {
                            await categoriaService.CreateCategoria(categoria);
                        }

                        var newHousing = HousingMock.GenerateOneHabitacaoMock(locadores);
                        int categoryIndex = 0; // Initialize an index for the categories

                        foreach (var habitacao in newHousing)
                        {
                            var selectedCategory = newCategories[categoryIndex]; // Select a category using the index
                            var newHousingCategoryRelation = HousingMock.GenerateHabitacaoCategoria(habitacao, selectedCategory);

                            habitacao.Categorias = new List<HabitacaoCategoria>() { newHousingCategoryRelation };

                            await housingService.CreateHabitacao(habitacao);

                            categoryIndex = (categoryIndex + 1) % newCategories.Count; // Increment the index and loop back if it reaches the end
                        }

                    // Utiliza o email do cliente existente
                    const string clienteEmail = "cliente@isec.pt";

                        var user = await userManager.FindByEmailAsync(clienteEmail);
                        if (user != null && await userManager.IsInRoleAsync(user, "Cliente"))
                        {
                            {
                                // Encontra um funcionário e uma habitação para a reserva
                                var funcionario = await userManager.FindByEmailAsync("funcionario@funcionario.com"); 
                                var habitacao = await housingService.GetHabitacao(1); 

                                //if (funcionario != null)
                                //{
                                //    var novaReserva = new Reserva
                                //    {
                                //        Funcionario = funcionario,
                                //        Cliente = user,
                                //        Habitacao = habitacao,
                                //        DataInicio = DateTime.Now,
                                //        DataFim = DateTime.Now.AddDays(7),
                                //        Estado = EstadoReserva.Aceite

                                //   };

                                //    await reservaService.CreateReserva(novaReserva);

                                //    var novoRegistoEntrega = new RegistoEntrega
                                //    {
                                //        DataEntrega = DateTime.Now,
                                //        Danos = false,
                                //        TipoTransacao = TipoTransacao.Entrega,
                                //        Funcionario = funcionario,
                                //        Observacoes = "nada"
                                //    };
                                    
                                //    novaReserva.RegistoEntregas = new List<RegistoEntrega>();
                                //    novaReserva.RegistoEntregas.Add(novoRegistoEntrega);
                                //    var updatedReserva = await reservasService.UpdateReserva(novaReserva, novaReserva.Id);

                                //    if(updatedReserva != null)
                                //    {
                                //        Console.WriteLine("Registo de entrega criado com sucesso parcial");
                                //        // atualizou
                                //    }

                                
                                //else
                                //{
                                //    throw new Exception();
                                //}
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());  
            }
        }
    }
}
