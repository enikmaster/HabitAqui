using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
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

                var mockReservas = ReservaMock.GenerateReservasMocks(users, habitacoes, locadores);

                foreach (var mockReserva in mockReservas)
                {
                    context.Reservas.Add(mockReserva);
                    await context.SaveChangesAsync();
                }

                //var mockRegistoEntregas = ReservaMock.GenerateRegistoEntregaMocks(mockReservas);

                //foreach (var mockRegistoEntrega in mockRegistoEntregas)
                //{
                //    context.RegistoEntregas.Add(mockRegistoEntrega);
                //    await context.SaveChangesAsync();
                //}
            }
        }
    }
}