using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Services;

public class ImagemService
{
    private readonly ApplicationDbContext _context;
    public ImagemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Imagem>> RemoverImagem(Imagem imagem, int habitacaoId)
    {
        var habitacao = await _context.Habitacoes
            .Include(h => h.Imagens)
            .FirstOrDefaultAsync(h => h.Id == habitacaoId);
        if (habitacao == null) return null;
        habitacao.Imagens.Remove(imagem);
        _context.Imagens.Remove(imagem);
        await _context.SaveChangesAsync();
        return habitacao.Imagens.ToList();
    }
}