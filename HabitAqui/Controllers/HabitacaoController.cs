using HabitAqui.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Controllers;

public class HabitacaoController : Controller
{
    private readonly ApplicationDbContext _context;

    public HabitacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}