using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Services;

public class HabitacaoService
{
    private readonly ApplicationDbContext _context;

    public HabitacaoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Habitacao?> GetHabitacao(int? id)
    {
        var habitacao = await _context.Habitacoes
            .Include(h => h.DetalhesHabitacao)
            .Include(h => h.Categorias)
            .Include(h => h.Imagens)
            .Include(h => h.Avaliacoes)
            .Include(h => h.Reservas)
            .FirstOrDefaultAsync(h => h.Id == id);
        return habitacao ?? null;
    }

    public async Task<List<Habitacao>> GetAllActiveHabitacoes()
    {
        return await _context.Habitacoes
            .Include(h => h.DetalhesHabitacao)
            .Include(h => h.Categorias)
            .Include(h => h.Imagens)
            .Include(h => h.Avaliacoes)
            .Include(h => h.Reservas)
            .Where(h => h.Active == true)
            .ToListAsync();
    }

    public async Task<List<Habitacao>> GetAllHabitacoesLocador(string userId)
    {
        var locador = await _context.Locadores
            .Include(l => l.Habitacoes)
            .FirstOrDefaultAsync(l => l.Administradores.Any(a => a.Id == userId));
        return locador == null
            ? await GetAllActiveHabitacoes()
            : locador.Habitacoes.ToList();
    }

    public async Task CreateHabitacao(Habitacao habitacao)
    {
        _context.Add(habitacao);
        await _context.SaveChangesAsync();
    }
}