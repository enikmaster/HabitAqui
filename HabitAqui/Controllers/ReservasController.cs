using System.Security.Claims;
using HabitAqui.Data;
using HabitAqui.Dtos.Reservas;
using HabitAqui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HabitAqui.Controllers;

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
            .Include(r => r.Habitacao.DetalhesHabitacao)
            .Where(r => r.RegistoEntregas.Any(re => re.TipoTransacao == TipoTransacao.Entrega))
            .ToListAsync();

        return View("Arrendamentos", arrendamentosAtivos);
    }

    public IActionResult Historico()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
        if (id == null) return NotFound();

        var reserva = await _context.Reservas
            .Include(r => r.Funcionario)
            .Include(r => r.Cliente)
            .Include(r => r.Habitacao)
            .Include(r => r.RegistoEntregas)
            .Include(r => r.Habitacao.DetalhesHabitacao) // Inclua os detalhes da habitação
            .FirstOrDefaultAsync(m => m.Id == id);

        if (reserva == null) return NotFound();

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

    public IActionResult ListarReservasCliente()
    {

        //reservas aceites
        //registoEntrega != devolucao   
        var reservasCliente = _context.Reservas
            .Include(r => r.Habitacao)
            .Include(r => r.Habitacao.DetalhesHabitacao)
            .Include(r => r.RegistoEntregas)
            .Where(r => r.RegistoEntregas.Any(re => re.TipoTransacao != TipoTransacao.Devolucao) && r.Estado == EstadoReserva.Aceite)
            .ToList();



        return View("RegistoCliente", reservasCliente);
    }


    // GET: Reservas/Reservar
    //[Authorize]
    public async Task<IActionResult> Reservar(int? id)
    {
        if (id == null) return NotFound();

        var habitacao = await _context.Habitacoes
            .Include(h => h.DetalhesHabitacao)
            .SingleOrDefaultAsync(h => h.Id == id);

        if (habitacao == null) return NotFound();

        var viewModel = new ReservaDto
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
    public IActionResult ConfirmarReserva(ReservaDto reservaDto)
    {
        if (ModelState.IsValid)
        {
            var dataInicio = reservaDto.DataInicio;
            var dataFim = reservaDto.DataFim;
            var AnotacoesCliente = reservaDto.AnotacoesCliente;
            

            var numeroNoites = (int)(dataFim - dataInicio).TotalDays;

            if (numeroNoites < 1)
            {
                ModelState.AddModelError(string.Empty, "A estadia deve ser de pelo menos uma noite.");
                return View("EfetuarReserva", reservaDto);
            }

            var habitacao = _context.Habitacoes
                .Where(h => h.Id == reservaDto.HabitacaoId)
                .Include(h => h.DetalhesHabitacao)
                .FirstOrDefault();

            if (habitacao == null) return NotFound();


            var precoTotal = numeroNoites * habitacao.DetalhesHabitacao.PrecoPorNoite;

            var confirmacaoViewModel = new ReservaDto
            {
                HabitacaoId = reservaDto.HabitacaoId,
                NomeHabitacao = reservaDto.NomeHabitacao,
                DescricaoHabitacao = reservaDto.DescricaoHabitacao,
                PrecoPorNoiteHabitacao = habitacao.DetalhesHabitacao.PrecoPorNoite,
                NumeroNoites = numeroNoites,
                DataInicio = dataInicio,
                DataFim = dataFim,
                precoTotal = precoTotal,
                AnotacoesCliente = AnotacoesCliente
            };

            return View("ConfirmarReserva", confirmacaoViewModel);
        }

        // Se o modelo não for válido, retorne à página de reserva
        return View("EfetuarReserva", reservaDto);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinalizarReserva(ReservaDto reservaDto)
    {
        var habitacao = _context.Habitacoes
            .Where(h => h.Id == reservaDto.HabitacaoId)
            .Include(h => h.DetalhesHabitacao)
            .Include(h => h.Avaliacoes)
            .Include(h => h.Categorias)
            .Include(h => h.Reservas)
            .FirstOrDefault();

        var locador = _context.Locadores
            .Where(u => u.Id == habitacao.LocadorId)
            .Include(u => u.Administradores)
            .FirstOrDefault();

        if (locador != null && locador.Administradores.Any())
        {
            var random = new Random();
            var randomIndex = random.Next(locador.Administradores.Count); // Get a random index
            var selectedAdministrador = locador.Administradores.ElementAt(randomIndex); // Retrieve the administrador at the random index
            if (ModelState.IsValid)
            {
                var novaReserva = new Reserva
                {
                    //Como vou buscar o funcionario
                    Estado = EstadoReserva.Pendente,
                    ClienteId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    FuncionarioId = selectedAdministrador.Id,
                    //Id = reservaDto.HabitacaoId,
                    DataInicio = reservaDto.DataInicio,
                    DataFim = reservaDto.DataFim,
                    Habitacao = habitacao
                };

                var numeroNoites = (int)(novaReserva.DataFim - novaReserva.DataInicio).TotalDays;

                // Agora você pode adicionar a nova reserva ao contexto e salvar as alterações
                _context.Reservas.Add(novaReserva);
                await _context.SaveChangesAsync();

                // Redirecione para a página de sucesso ou outra página desejada
                return View("Index");
            }
        }
        else return NotFound();
        // Se houver erros de validação, retorne para a página de confirmação com os erros
        return View(reservaDto);
    }
}