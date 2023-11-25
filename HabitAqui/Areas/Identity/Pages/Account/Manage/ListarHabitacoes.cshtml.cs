using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class ListarHabitacoesModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public ListarHabitacoesModel(ApplicationDbContext context)
    {
        _context = context;
    }
    public IList<Habitacao> Habitacoes { get; set; }
    public void OnGet()
    {
        Habitacoes = _context.Habitacoes.ToList();
    }
}
