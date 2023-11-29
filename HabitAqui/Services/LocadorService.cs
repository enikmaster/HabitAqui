using HabitAqui.Data;
using HabitAqui.Models;

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
}