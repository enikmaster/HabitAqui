using HabitAqui.Areas.Identity.Pages.Account.Manage;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    
    public IActionResult Detalhes(int id)
    {
        Locador locador = _locadorService.GetLocador(id);
        if (locador == null)
            return RedirectToAction("Index", "Home");
        return View(locador);
    }
}