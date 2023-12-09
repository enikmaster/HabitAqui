using System.Security.Claims;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
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

    public HabitacaoController(ApplicationDbContext context,
        HabitacaoService habitacaoService,
        CategoriaService categoriaService)
    {
        _context = context;
        _habitacaoService = habitacaoService;
        _categoriaService = categoriaService;
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
        if (id == null) return NotFound();
        var habitacao = await _habitacaoService.GetHabitacao(id);
        if (habitacao == null) return NotFound();

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
}