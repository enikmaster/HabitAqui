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
            .Include(l => l.Localizacao)
            .Include(h => h.Habitacoes)
            .Include(a => a.Administradores)
            .FirstOrDefaultAsync(l => l.Id == id);
        return locador ?? null;
    }

    public async Task<Locador> GetLocadorGestor(string id)
    {
        var locador = await _context.Locadores
            .Include(l => l.Localizacao)
            .Include(h => h.Habitacoes)
            .Include(a => a.Administradores)
            .FirstOrDefaultAsync(l => l.Administradores.Any(a => a.Id == id));
        return locador ?? null;
    }

    public async Task<Locador?> GetLocadorByEmail(string email)
    {
        var locador = _context.Locadores
            .Where(x => x.Email == email)
            .FirstOrDefault();
        return locador;
    }

    public Locador UpdateLocador(Locador locador)
    {
        try
        {
            var currentlocador = _context.Locadores.Where(l => l.Id == locador.Id).Include(x => x.Localizacao)
                .FirstOrDefault();
            if (currentlocador == null) return null;

            currentlocador.Nome = locador.Nome;
            currentlocador.Apelido = locador.Apelido;
            currentlocador.Email = locador.Email;
            currentlocador.PhoneNumber = locador.PhoneNumber;
            currentlocador.Nif = locador.Nif;
            //currentlocador.EstadoDaSubscricao = locador.EstadoDaSubscricao;
            // currentlocador.Administradores = locador.Administradores;
            //tratar a localizaçao objeto
            currentlocador.Localizacao.Cidade = locador.Localizacao.Cidade;
            currentlocador.Localizacao.Pais = locador.Localizacao.Pais;
            currentlocador.Localizacao.CodigoPostal = locador.Localizacao.CodigoPostal;
            currentlocador.Localizacao.Morada = locador.Localizacao.Morada;


            _context.Update(currentlocador);
            _context.SaveChanges();

            return currentlocador;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ApplicationException("Erro de concorrência ao atualizar o locador.", ex);
        }
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