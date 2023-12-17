using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReservasService
{
    private readonly ApplicationDbContext _context;

    public ReservasService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reserva> GetReservaById(int reservaId)
    {
        var reservas =  await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Funcionario)
            .Include(r => r.Habitacao)
            .Include(r => r.RegistoEntregas)
            .FirstOrDefaultAsync(r => r.Id == reservaId);

        return reservas;
    }

    public async Task<IEnumerable<Reserva>> GetReservasByClienteId(string clienteId)
    {
        return await _context.Reservas
            .Where(r => r.ClienteId == clienteId)
            .Include(r => r.Habitacao)
            .Include(r => r.Funcionario)
            .Include(r => r.RegistoEntregas)
            .ToListAsync();
    }

    public async Task<Reserva> CreateReserva(Reserva newReserva)
    {
        _context.Reservas.Add(newReserva);
        await _context.SaveChangesAsync();
        return newReserva;
    }

    public async Task<Reserva> UpdateReserva(Reserva updatedReserva)
    {
        var reserva = await _context.Reservas
            .FirstOrDefaultAsync(r => r.Id == updatedReserva.Id);

        if (reserva == null) return null;

        reserva.DataInicio = updatedReserva.DataInicio;
        reserva.DataFim = updatedReserva.DataFim;
        reserva.Estado = updatedReserva.Estado;
        reserva.FuncionarioId = updatedReserva.FuncionarioId;
        reserva.ClienteId = updatedReserva.ClienteId;
        reserva.Habitacao = updatedReserva.Habitacao;
        // Update other fields as necessary

        _context.Update(reserva);
        await _context.SaveChangesAsync();
        return reserva;
    }

    public async Task DeleteReserva(int reservaId)
    {
        var reserva = await _context.Reservas.FindAsync(reservaId);
        if (reserva != null)
        {
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
        }
    }

    // Additional methods as needed for your specific requirements
}
