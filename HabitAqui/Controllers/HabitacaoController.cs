using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers;

public class HabitacaoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<DetalhesHabitacao> detalhesHabitacao;

    public HabitacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Habitacao
    public async Task<IActionResult> Index()
    {
        return _context.Habitacoes != null
            ? View(await _context.Habitacoes.ToListAsync())
            : Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
    }

    // GET: Habitacao/Details/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null || _context.Habitacoes == null) return NotFound();

        var habitacao = await _context.Habitacoes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (habitacao == null) return NotFound();

        return View(habitacao);
    }

    // GET: Habitacao/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Habitacao/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id")] Habitacao habitacao)
    {
        if (ModelState.IsValid)
        {
            _context.Add(habitacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(habitacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacaoExists(habitacao.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(habitacao);
    }

    // GET: Habitacao/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Habitacoes == null) return NotFound();

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
        if (_context.Habitacoes == null) return Problem("Entity set 'ApplicationDbContext.Habitacoes'  is null.");
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