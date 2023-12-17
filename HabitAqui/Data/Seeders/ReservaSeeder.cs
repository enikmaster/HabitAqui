using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public static class ReservaSeeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<DetalhesUtilizador>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
           // var locador = serviceProvider.GetRequiredService<Locador>();

            if (!context.Reservas.Any())
            {
                var users = await context.DetalhesUtilizadores.ToListAsync();

                var habitacoes = await context.Habitacoes
                                            .Include(habitacoes => habitacoes.DetalhesHabitacao)
                                            .Include(habitacoes => habitacoes.Locador)
                                            .ToListAsync();

                var locadores = await context.Locadores
                                             .Include(locadores => locadores.Administradores)
                                             .ToListAsync();

                var mockReservas =  ReservaMock.GenerateReservasMocks(users, habitacoes, locadores);

                foreach(var mockReserva in mockReservas)
                {
                    context.Reservas.Add(mockReserva);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
