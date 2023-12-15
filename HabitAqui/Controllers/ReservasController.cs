using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using HabitAqui.ViewModels.Reservas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        // GET: Reservas/ListarReservasFuncionario
        [Authorize(Roles = "Gestor")]
        public IActionResult ListarReservas()
        {
            var funcionarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservasFuncionario = _context.Reservas
                .Include(r => r.Habitacao.DetalhesHabitacao)
                .Include(r => r.RegistoEntregas)
                .Where(r => r.FuncionarioId == funcionarioId)
                .ToList();
            return View(reservasFuncionario);
        }


        // GET: Reservas/Reservar
        //[Authorize]
        public async Task<IActionResult> Reservar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacao = await _context.Habitacoes
                .Include(h => h.DetalhesHabitacao)
                .SingleOrDefaultAsync(h => h.Id == id);

            if (habitacao == null)
            {
                return NotFound();
            }

            var viewModel = new EfetuarReserva
            {
                HabitacaoId = habitacao.Id,
                NomeHabitacao = habitacao.DetalhesHabitacao.Nome,
                DescricaoHabitacao = habitacao.DetalhesHabitacao.Descricao,
                PrecoPorNoiteHabitacao = habitacao.DetalhesHabitacao.PrecoPorNoite
            };

            return View("EfetuarReserva", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarReserva(EfetuarReserva viewModel)
        {
            if (ModelState.IsValid)
            {
                var dataInicio = viewModel.DataInicio;
                var dataFim = viewModel.DataFim;
                var AnotacoesCliente = viewModel.AnotacoesCliente;

                var numeroNoites = (int)(dataFim - dataInicio).TotalDays;

                if (numeroNoites < 1)
                {
                    ModelState.AddModelError(string.Empty, "A estadia deve ser de pelo menos uma noite.");
                    return View("EfetuarReserva", viewModel);
                }

                var habitacao = _context.Habitacoes
                    .Where(h => h.Id == viewModel.HabitacaoId)
                    .Include(h => h.DetalhesHabitacao)
                    .FirstOrDefault();

                if(habitacao == null)
                {
                    return NotFound();
                }



                var precoTotal = numeroNoites * habitacao.DetalhesHabitacao.PrecoPorNoite;

                var confirmacaoViewModel = new EfetuarReserva
                {
                    HabitacaoId = viewModel.HabitacaoId,
                    NomeHabitacao = viewModel.NomeHabitacao,
                    DescricaoHabitacao = viewModel.DescricaoHabitacao,
                    PrecoPorNoiteHabitacao = habitacao.DetalhesHabitacao.PrecoPorNoite, 
                    NumeroNoites= numeroNoites,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    precoTotal = precoTotal,
                    AnotacoesCliente = AnotacoesCliente
                };

                return View("ConfirmarReserva", confirmacaoViewModel);
            }

            // Se o modelo não for válido, retorne à página de reserva
            return View("EfetuarReserva", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizarReserva(EfetuarReserva viewModel)
        {


            var habitacao = _context.Habitacoes
                    .Where(h => h.Id == viewModel.HabitacaoId)
                    .Include(h => h.DetalhesHabitacao)
                    .Include(h => h.Avaliacoes)
                    .Include(h => h.Categorias)
                    .Include(h => h.Reservas)
                    .FirstOrDefault();

            var funcionario = _context.Users.FirstOrDefault("Gestor");

            if (ModelState.IsValid)
            {
                var novaReserva = new Reserva
                {
                    //Como vou buscar o funcionario
                    Estado = EstadoReserva.Pendente,
                    ClienteId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Id = viewModel.HabitacaoId,
                    DataInicio = viewModel.DataInicio,
                    DataFim = viewModel.DataFim,
                    Habitacao = habitacao
                    
                
                };

                var numeroNoites = (int)(novaReserva.DataFim - novaReserva.DataInicio).TotalDays;

                // Agora você pode adicionar a nova reserva ao contexto e salvar as alterações
                _context.Reservas.Add(novaReserva);
                await _context.SaveChangesAsync();

                // Redirecione para a página de sucesso ou outra página desejada
                return RedirectToAction("ReservaConfirmada");
            }

            // Se houver erros de validação, retorne para a página de confirmação com os erros
            return View(viewModel);
        }



    }




}
