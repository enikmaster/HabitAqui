using System.Security.Claims;
using HabitAqui.Data;
using HabitAqui.Dtos.Reservas;
using HabitAqui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers;

public class ReservasController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public ReservasController(ApplicationDbContext context, UserManager<DetalhesUtilizador> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    [Authorize(Roles = "Gestor, Funcionario")]
    public IActionResult ListarReservas()
    {
        var funcionarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var reservasFuncionario = _context.Reservas
            .Include(r => r.Habitacao.DetalhesHabitacao)
            .Include(r => r.RegistoEntregas)
            .Where(r => r.FuncionarioId == funcionarioId && r.Estado != EstadoReserva.Concluido)
            .ToList();
        return View("RegistoCliente", reservasFuncionario);
    }

    // GET: Reservas/ListarReservasFuncionario
    [Authorize(Roles = "Gestor, Funcionario")]
    public IActionResult ListarCliente()
    {
        var funcionarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var reservasCliente = _context.Reservas
           .Include(r => r.Habitacao)
           .Include(r => r.Habitacao.DetalhesHabitacao)
           .Include(r => r.RegistoEntregas)
           .Where(r => r.Estado != EstadoReserva.Aceite && r.Estado != EstadoReserva.Pendente)
           .ToList();


        ViewData["Title"] = "Reservas Terminadas";

        return View("RegistoCliente", reservasCliente);

    }

    public IActionResult ListarReservasCliente()
    {
        //reservas aceites
        //registoEntrega != devolucao   
        var reservasCliente = _context.Reservas
            .Include(r => r.Habitacao)
            .Include(r => r.Habitacao.DetalhesHabitacao)
            .Include(r => r.RegistoEntregas)
            .Where(r => r.Estado != EstadoReserva.Concluido)
            .ToList();

        ViewData["Title"] = "Reservas Ativas/Pendentes";
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
    public IActionResult ConfirmarReserva(ReservaGeraldto reservaDto)
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
                NomeHabitacao = habitacao.DetalhesHabitacao.Nome,
                DescricaoHabitacao = habitacao.DetalhesHabitacao.Descricao,
                PrecoPorNoiteHabitacao = habitacao.DetalhesHabitacao.PrecoPorNoite,
                NumeroNoites = numeroNoites,
                DataInicio = dataInicio,
                DataFim = dataFim,
                precoTotal = precoTotal,
                AnotacoesCliente = AnotacoesCliente
            };
            return View("ConfirmarReserva", confirmacaoViewModel);
        }

        return View("EfetuarReserva", reservaDto);
    }

    //USER CONFIRMA A RESERVA, FICA PENDENTE
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
            var selectedAdministrador =
                locador.Administradores.ElementAt(randomIndex); // Retrieve the administrador at the random index
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

                _context.Reservas.Add(novaReserva);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Habitacao");
            }
        }
        else
        {
            return NotFound();
        }

        return View(reservaDto);
    }

    public IActionResult FuncEntregaReserva(int id)
    {
        var reserva = _context.Reservas
            .FirstOrDefault(u => u.Id == id);

        var funcionarioId = reserva.FuncionarioId;

        var entregaDto = new EntregaDto
        {
            FuncionarioId = funcionarioId,
            DataEntrega = DateTime.Now,
            TipoTransacao = TipoTransacao.Entrega,
            ReservaId = reserva.Id,
            Danos = false,
            Observacoes = "Nenhuma observação"

        };

        return View("FuncEntregaReserva", entregaDto);
    }

    public async Task<IActionResult> FinTeste(EntregaDto entregaDto)
    {
        var reserva = _context.Reservas
            .Include(r => r.RegistoEntregas)
            .FirstOrDefault(u => u.Id == entregaDto.ReservaId);

        if (reserva == null)
        {
            return NotFound();
        }

        var funcionario = await _userManager.GetUserAsync(User);    
        var entregaDtoFinal = new RegistoEntrega
        {
            Funcionario = funcionario,
            DataEntrega = entregaDto.DataEntrega,
            TipoTransacao = entregaDto.TipoTransacao,
            Observacoes = entregaDto.Observacoes,
            Danos = entregaDto.Danos,
        };

        if(entregaDtoFinal.TipoTransacao == TipoTransacao.Entrega)
        {
            reserva.Estado = EstadoReserva.Aceite;
        }else if(entregaDto.TipoTransacao == TipoTransacao.Devolucao)
        {
            reserva.Estado = EstadoReserva.Concluido;
        }
        else
        {
            reserva.Estado = EstadoReserva.Rejeitado;
        }


        reserva.RegistoEntregas.Add(entregaDtoFinal);
        _context.SaveChanges();


        return RedirectToAction("Index", "Habitacao");
    }



    public async Task<IActionResult> EntregarHabitacaoAsync(int id)
    {
        var reserva = _context.Reservas
            .FirstOrDefault(r => r.Id == id);
        if (reserva == null) return NotFound(); // Retorna uma resposta NotFound caso a reserva não seja encontrada
        var funcionario = await _userManager.GetUserAsync(User);

        // Crie uma nova instância do objeto RegistoEntrega
        var registoEntrega = new RegistoEntrega
        {
            Funcionario = funcionario,
            DataEntrega = DateTime.Now,
            TipoTransacao = TipoTransacao.Entrega,
            Danos = false,
            Observacoes =
                "Nenhuma observação"
        };

        if (reserva.RegistoEntregas == null) reserva.RegistoEntregas = new List<RegistoEntrega>();
        reserva.RegistoEntregas.Add(registoEntrega);

        _context.SaveChanges();

        return RedirectToAction("Detalhes", new { id });
    }


 


}