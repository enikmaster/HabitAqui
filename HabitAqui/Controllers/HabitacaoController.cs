using System.Security.Claims;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using HabitAqui.ViewModels.Avaliacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers;

public class HabitacaoController : Controller
{
    private readonly CategoriaService _categoriaService;
    private readonly ApplicationDbContext _context;
    private readonly HabitacaoService _habitacaoService;
    private readonly UserManager<DetalhesHabitacao> detalhesHabitacao;
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public HabitacaoController(ApplicationDbContext context,
       HabitacaoService habitacaoService,
       CategoriaService categoriaService,
       UserManager<DetalhesUtilizador> userManager)

    {
        _context = context;
        _habitacaoService = habitacaoService;
        _categoriaService = categoriaService;
        _userManager = userManager;
    }
    // TODO: Index - feito
    // TODO: Create
    // TODO: Update/Edit
    // TODO: Delete

    // GET: Habitacao
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole(Roles.Funcionario.ToString()) || User.IsInRole(Roles.Gestor.ToString()))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _habitacaoService.GetAllHabitacoesLocador(userId));
            /*var locador = await _context.Locadores
                .Include(l => l.Habitacoes)
                .FirstOrDefaultAsync(l => l.Administradores.Any(a => a.Id == userId));
            return locador == null
                ? View(await _context.Habitacoes.Where(h => h.Active == true).ToListAsync())
                : View(locador.Habitacoes);*/
            /*return View(await _context.Habitacoes
                .Where(h => h.Locador.Id == locador.Id).ToListAsync());*/
        }

        return View(await _habitacaoService.GetAllActiveHabitacoes());
    }

    // GET: Habitacao/Details/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var habitacao = await _context.Habitacoes
            .Include(h => h.Avaliacoes) // Carrega as avaliações relacionadas
                .ThenInclude(a => a.Cliente) // Inclui informações do cliente que fez a avaliação
            .FirstOrDefaultAsync(m => m.Id == id);

        if (habitacao == null)
        {
            return NotFound();
        }

        return View(habitacao);
    }

    // GET: Habitacao/Create
    public async Task<IActionResult> Create()
    {
        var categorias = await _categoriaService.GetAllActive();
        ViewBag.Categorias = categorias.Any() ? categorias : null;
        return View();
    }

    // POST: Habitacao/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome")] Habitacao habitacao)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _habitacaoService.CreateHabitacao(habitacao);
                //_context.Add(habitacao);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Não foi possível guardar as alterações. " +
                                         "Tente novamente e, se o problema persistir, " +
                                         "consulte o administrador do sistema.");
        }

        return View(habitacao);
    }

    // GET: Habitacao/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Habitacoes == null) return NotFound();

        var habitacao = await _context.Habitacoes.FindAsync(id);
        if (habitacao == null) return NotFound();
        return View(habitacao);
    }

    // POST: Habitacao/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id")] Habitacao habitacao)
    {
        if (id != habitacao.Id) return NotFound();

        if (!ModelState.IsValid) return View(habitacao);
        try
        {
            _context.Update(habitacao);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if ((_context.Habitacoes?.Any(e => e.Id == habitacao.Id)).GetValueOrDefault())
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Habitacao/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var habitacao = await _context.Habitacoes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (habitacao == null) return NotFound();

        return View(habitacao);
    }

    // POST: Habitacao/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Habitacoes == null) return Problem("Entity set 'ApplicationDbContext.Habitacoes' is null.");
        var habitacao = await _context.Habitacoes.FindAsync(id);
        if (habitacao != null) _context.Habitacoes.Remove(habitacao);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HabitacaoExists(int id)
    {
        return (_context.Habitacoes?.Any(e => e.Id == id)).GetValueOrDefault();
    }


    // GET: Habitacao/Avaliar/5
    public async Task<IActionResult> Avaliar(int? id)
    {
        if (id == null) return NotFound();

        var habitacao = await _habitacaoService.GetHabitacao(id.Value);
        if (habitacao == null) return NotFound();

        var avaliacao = new Avaliacao { HabitacaoId = habitacao.Id };
        return View("Avaliacao", avaliacao); // Assegura que o nome da view corresponde ao arquivo da view
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] // TODO: ESPECIFICAR A ROLE CLIENTE <-  Roles="Cliente"
    public async Task<IActionResult> Avaliar(int id, NovaAvaliacao avaliacao)
    {
        // Se o ModelState for válido, salva a avaliação
        if (ModelState.IsValid)
        {
            var habitacao = await _habitacaoService.GetHabitacao(avaliacao.HabitacaoId);
            
            // habitacao nao existe ?
            if (habitacao == null)
            {
                return BadRequest();
            }

            // TODO: verificar se  ->>>> o lease dele já tem avaliação ? então bad request
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return BadRequest();

            var novaAvalicaoObj = new Avaliacao()
            {
                Cliente = currentUser,
                Comentario = avaliacao.Comentario,
                HabitacaoId = habitacao.Id,
                Nota = avaliacao.Nota,
            };

            // criar avaliacao
            var novaAvalicao = _context.Avaliacoes.Add(novaAvalicaoObj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalhes), new { id = avaliacao.HabitacaoId });
        }

        // Se o ModelState não for válido, loga os erros e recarrega a view
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                Console.WriteLine(erro.ErrorMessage); // Logar os erros
            }
        }

        // Recarregar a view de avaliação
        return View("Avaliacao", avaliacao);
    }

   


}


