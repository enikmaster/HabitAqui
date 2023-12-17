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
            .Include(l => l.Locador)
            .Include(h => h.DetalhesHabitacao)
            .ThenInclude(l => l.Localizacao)
            .Include(h => h.Categorias)
            .Include(h => h.Imagens)
            .Include(h => h.Avaliacoes)!
            .ThenInclude(h => h.Cliente)
            .Include(h => h.Reservas)
            .FirstOrDefaultAsync(h => h.Id == id);
        return habitacao ?? null;
    }

    public async Task<Habitacao?> GetHabitacaoReservasPaginadas(int? id, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        var habitacao = await _context.Habitacoes
            .Include(h => h.DetalhesHabitacao)
            .Include(h => h.Categorias)
            .Include(h => h.Imagens)
            .Include(h => h.Avaliacoes!
                .OrderBy(a => a.Id)
                .Skip(skip)
                .Take(pageSize))
            .ThenInclude(a => a.Cliente)
            .Include(h => h.Reservas)
            .FirstOrDefaultAsync(h => h.Id == id);
        return habitacao ?? null;
    }


    public async Task<List<Habitacao>> GetAllActiveHabitacoes()
    {
        return await _context.Habitacoes
            .Include(h => h.DetalhesHabitacao)
            .ThenInclude(x => x.Localizacao)
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
            .Include(h => h.Habitacoes)!
            .ThenInclude(i => i.Imagens)
            .Include(h => h.Habitacoes)!
            .ThenInclude(d => d.DetalhesHabitacao)
            .ThenInclude(l => l.Localizacao)
            .FirstOrDefaultAsync(l => l.Id == userId);
        return locador == null
            ? await GetAllActiveHabitacoes()
            : locador.Habitacoes.ToList();
    }

    public async Task CreateHabitacao(Habitacao habitacao)
    {
        _context.Add(habitacao);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Avaliacao>> GetAvaliacoesDaHabitacao(int habitacaoId)
    {
        var habitacao = await _context.Habitacoes
            .Include(h => h.Avaliacoes)
            .FirstOrDefaultAsync(h => h.Id == habitacaoId);

        if (habitacao != null) return habitacao.Avaliacoes.ToList();

        return new List<Avaliacao>();
    }
}