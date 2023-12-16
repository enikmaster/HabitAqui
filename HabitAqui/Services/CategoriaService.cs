using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Services;

public class CategoriaService
{
    private readonly ApplicationDbContext _context;

    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAll()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<List<Categoria>> GetAllActive()
    {
        return await _context.Categorias
            .Where(c => c.Active == true)
            .ToListAsync();
    }


    public async Task<Categoria> CreateCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria> GetCategoria(int categoriaId)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == categoriaId);
        return categoria ?? null;
    }
}