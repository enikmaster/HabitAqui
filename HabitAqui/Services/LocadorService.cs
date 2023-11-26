using HabitAqui.Data;
using HabitAqui.Models;
using PagedList;

namespace HabitAqui.Services;

public class LocadorService
{
    private readonly ApplicationDbContext _context;
    
    public LocadorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Locador GetLocador(int id)
    {
        return _context.Locadores.Find(id);
    }
    
    public IPagedList<Locador> GetLocadoresPaginados(int page, int pageSize)
    {
        return _context.Locadores.AsQueryable().ToPagedList(page, pageSize);
    }
}