using HabitAqui.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Certifique-se de substituir pelo namespace do seu contexto de dados

namespace HabitAqui.Controllers;

public class FuncionarioController : Controller
{
    private readonly ApplicationDbContext _context; // Substitua pelo seu contexto de dados

    public FuncionarioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // esta view seria a view com a lista dos funcionários, que só é acedida pelos Gestores
        return View();
    }

    public IActionResult ListarReservas()
    {
        var reservas = _context.Reservas
            .Include(r => r.Funcionario)
            .Include(r => r.Cliente)
            .Include(r => r.Habitacao)
            .ToList();

        if (reservas == null) return NotFound();

        return View(reservas);
    }
}