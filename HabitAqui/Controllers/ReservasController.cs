using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Arrendamentos()
        {
            var arrendamentosAtivos = await _context.Reservas
                .Include(r => r.Funcionario)
                .Include(r => r.Cliente)
                .Include(r => r.Habitacao)
                .Include(r => r.RegistoEntregas)
                .Where(r => r.RegistoEntregas.Any(re => re.TipoTransacao == TipoTransacao.Entrega))
                .ToListAsync();

            return View("Arrendamentos", arrendamentosAtivos);
        }



        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Funcionario)
                .Include(r => r.Cliente)
                .Include(r => r.Habitacao)
                .ToListAsync();

            return View(reservas);
        }


        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Funcionario)
                .Include(r => r.Cliente)
                .Include(r => r.Habitacao)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }
    }
}
