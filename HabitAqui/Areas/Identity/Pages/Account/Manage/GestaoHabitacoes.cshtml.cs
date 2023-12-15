using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class GestaoHabitacoesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public GestaoHabitacoesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Habitacao> Habitacoes { get; set; }
    public IList<Locador> Locadores { get; set; }

    public void OnGet()
    {
        Habitacoes = _context.Habitacoes
            .Include(d => d.DetalhesHabitacao)
            .ToList();
        Locadores = _context.Locadores.ToList();
    }

    public string GetLocadorName(Habitacao habitacao)
    {
        // procurar na tabela de locadores o locador que tem, na sua lista de habitacoes, a habitacao passada por argumento
        var locador = Locadores.FirstOrDefault(l => l.Habitacoes != null && l.Habitacoes.Contains(habitacao));
        return locador == null ? "Sem locador" : locador.Nome + " " + locador.Apelido;
    }
}