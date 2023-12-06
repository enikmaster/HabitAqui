using HabitAqui.Data;
using HabitAqui.Models;
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

    [HttpGet]
    public async Task<IActionResult> Detalhes(string id)
    {
        var locador = await _locadorService.GetLocador(id);
        if (locador == null)
            return RedirectToAction("Index", "Home");
        return View(locador);
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var locador = await _locadorService.GetLocador(id);
        if (locador == null)
            return RedirectToAction("Index", "Home");
        return View(locador);
    }

    [HttpPost]
    public IActionResult Update(Locador locador)
    {
        if (!ModelState.IsValid) return View(locador);
        _locadorService.UpdateLocador(locador);
        var statusMessage = _locadorService.StatusMessage;
        if (!string.IsNullOrEmpty(statusMessage))
            ViewBag.StatusMessage = statusMessage;

        return RedirectToAction("Detalhes", new { id = locador.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var locador = await _locadorService.GetLocador(id);
        if (locador == null)
            return Redirect("/Identity/Account/Manage/GestaoLocadores");
        return View(locador);
    }

    [HttpPost]
    public async Task<IActionResult> SoftDelete(string Id)
    {
        if (!string.IsNullOrEmpty(Id))
            await _locadorService.DeleteLocador(Id);
        return Redirect("/Identity/Account/Manage/GestaoLocadores");
    }
}