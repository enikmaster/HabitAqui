using HabitAqui.Areas.Identity.Pages.Account.Manage;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Services;

public class LocadorService
{
    private readonly ApplicationDbContext _context;

    public LocadorService(ApplicationDbContext context)
    {
        _context = context;
    }

    [TempData] public string StatusMessage { get; private set; } = string.Empty;

    public async Task<Locador> GetLocador(string id)
    {
        var locador = await _context.Locadores
            .Include(h => h.Habitacoes)
            .Include(a => a.Administradores)
            .FirstOrDefaultAsync(l => l.Id == id);
        return locador;
    }

    public Locador UpdateLocador(Locador locador)
    {
        _context.Locadores.Update(locador);
        _context.SaveChanges();
        return locador;
    }

    public async Task DeleteLocador(string locadorId)
    {
        var locador = await GetLocador(locadorId);
        if (locador.Habitacoes is { Count: > 0 })
        {
            StatusMessage = "Não é possível eliminar um locador com habitações associadas.";
            return;
        }

        locador.Active = false;
        locador.EstadoDaSubscricao = EstadoSubscricao.Cancelado.ToString();
        foreach (var administrador in locador.Administradores)
            administrador.Active = false;
        _context.Locadores.Update(locador);
        await _context.SaveChangesAsync();
    }

    /*public string GetStatusMessage()
    {
        return StatusMessage;
    }*/
}