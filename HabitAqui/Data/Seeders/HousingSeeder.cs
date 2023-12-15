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

            var reservaService = serviceProvider.GetRequiredService<ReservaService>();

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var locador = await locadorService.GetLocadorByEmail("locador@locador.com");
                if (locador != null)
                {

                    // verify if the housing already exists ( to not add it)
                    var housingsList = await housingService.GetAllHabitacoesLocador(locador.Id);

                   if (!housingsList.Any())
                    {
                        var newCategory = CategoriaMock.GenerateCategoria();
                        await categoriaService.CreateCategoria(newCategory);

                        var newHousing = HousingMock.GenerateOneHabitacaoMock(locador.Id);
                        foreach (var habitacao in newHousing)
                        {
                            var newHousingCategoryRelation = HousingMock.GenerateHabitacaoCategoria(habitacao, newCategory);

                            habitacao.Categorias = new List<HabitacaoCategoria>() { newHousingCategoryRelation };

                            await housingService.CreateHabitacao(habitacao);
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

                                if (funcionario != null)
                                {
                                    var novaReserva = new Reserva
                                    {
                                        Funcionario = funcionario,
                                        Cliente = user,
                                        Habitacao = habitacao,
                                        DataInicio = DateTime.Now,
                                        DataFim = DateTime.Now.AddDays(7),
                                        Estado = EstadoReserva.Aceite

                                   };

                                    await reservaService.CreateReserva(novaReserva);

                                    var novoRegistoEntrega = new RegistoEntrega
                                    {
                                        DataEntrega = DateTime.Now,
                                        Danos = false,
                                        TipoTransacao = TipoTransacao.Entrega,
                                        Funcionario = funcionario,
                                        Observacoes = "nada"
                                    };
                                    
                                    novaReserva.RegistoEntregas = new List<RegistoEntrega>();
                                    novaReserva.RegistoEntregas.Add(novoRegistoEntrega);
                                    var updatedReserva = await reservaService.UpdateReserva(novaReserva, novaReserva.Id);

                                    if(updatedReserva != null)
                                    {
                                        Console.WriteLine("Registo de entrega criado com sucesso parcial");
                                        // atualizou
                                    }

                                }
                                else
                                {
                                    throw new Exception();
                                }
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
