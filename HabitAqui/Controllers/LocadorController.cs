using System.Diagnostics;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HabitAqui.Data;
namespace HabitAqui.Controllers;

public class LocadorController : Controller
{
    private readonly LocadorService _locadorService;
    
    private readonly ApplicationDbContext _context;

    public LocadorController(LocadorService locadorService, ApplicationDbContext context)
    {
        _locadorService = locadorService;
        _context = context;

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
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            Console.WriteLine(errors);

            return View(locador);
        }


        // Adicione um log aqui
        Debug.WriteLine("A ação Update foi alcançada.");

        _locadorService.UpdateLocador(locador);
        var statusMessage = _locadorService.StatusMessage;
        if (!string.IsNullOrEmpty(statusMessage)) ViewBag.StatusMessage = statusMessage;

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
    public async Task<IActionResult> Delete(string Id, string value)
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var locador = await _locadorService.GetLocador(Id);

            if (locador == null)
            {
                // Handle the case where the locador does not exist
                // For example, return a not found result or set an error message
                return NotFound();
            }

            if (locador.Habitacoes != null && locador.Habitacoes.Any())
            {
                TempData["ErrorMessage"] = "Não é possível apagar o Locador porque está associado a uma ou mais habitações.";
                return View(locador);
            }

            // Delete Administradores associated with the Locador
            if (locador.Administradores != null)
            {
                foreach (var administrador in locador.Administradores)
                {
                    administrador.Active = false; // Set the Active property to false
                    await _context.SaveChangesAsync();
                }
                locador.Active = false;
                locador.EstadoDaSubscricao = "Desativo";
                _locadorService.UpdateLocador(locador);

            }

            
            
        }

        return Redirect("/Identity/Account/Manage/GestaoLocadores");
    }

}