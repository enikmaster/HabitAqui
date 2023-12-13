using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabitAqui.Services
{
    public class ReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva> CreateReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task<Reserva?> UpdateReserva(Reserva reservaObj, int id)
        {
           var reserva = _context.Reservas.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (reserva == null)
                return null;

            var updatedObj = _context.Reservas.Update(reservaObj);
            await _context.SaveChangesAsync();

            return reservaObj;
        }

        // Aqui podes adicionar outros métodos, como para buscar reservas, etc.
    }
}
