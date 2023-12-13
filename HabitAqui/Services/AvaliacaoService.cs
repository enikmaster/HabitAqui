using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Services
{
    public class AvaliacaoService
    {
        private readonly ApplicationDbContext _context;

        public AvaliacaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAvaliacao(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Avaliacao>> GetAvaliacoesByHabitacaoId(int habitacaoId)
        {
            return await _context.Avaliacoes
                .Where(a => a.Habitacao.Id == habitacaoId)
                .Include(a => a.Cliente)
                .ToListAsync();
        }

        // Outros métodos conforme necessário
    }
}