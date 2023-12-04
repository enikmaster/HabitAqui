using HabitAqui.Data;
using HabitAqui.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Controllers;

public class LocadorController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly LocadorService _locadorService;

    public LocadorController(ApplicationDbContext context, LocadorService locadorService)
    {
        _context = context;
        _locadorService = locadorService;
    }

    /*public IActionResult Create()
    {
        return View();
    }*/

    public IActionResult Detalhes(int id)
    {
        var locador = _locadorService.GetLocador(id);
        if (locador == null)
            return RedirectToAction("Index", "Home");
        return View(locador);
    }
}