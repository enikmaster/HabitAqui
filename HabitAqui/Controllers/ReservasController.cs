using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                .Include(r => r.Habitacao.DetalhesHabitacao) // Inclua os detalhes da habitação
                .Where(r => r.RegistoEntregas.Any(re => re.TipoTransacao == TipoTransacao.Entrega))
                .ToListAsync();

            return View("Arrendamentos", arrendamentosAtivos);
        }

        public IActionResult Historico()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lógica para obter os arrendamentos históricos aqui
            var historicoArrendamentos = _context.Reservas
                .Include(r => r.Funcionario)
                .Include(r => r.Cliente)
                .Include(r => r.Habitacao)
                .Where(r => r.DataFim > DateTime.Now && r.Cliente.Id == userId)
                .ToList();

            return View("HistoricoArrendamento", historicoArrendamentos);
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
                .Include(r => r.RegistoEntregas)
                .Include(r => r.Habitacao.DetalhesHabitacao) // Inclua os detalhes da habitação
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

       


    }
}
