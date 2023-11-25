using System.ComponentModel;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class GestaoLocadoresModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public GestaoLocadoresModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public InputModel Input { get; set; }
    public IList<Locador> Locadores { set; get; }

    public void OnGet()
    {
        Locadores = _context.Locadores.ToList();
        Input = new InputModel();
    }

    public class InputModel
    {
        [DisplayName("Nome do Locador")] public string NomeLocador { get; set; }

        [DisplayName("Estado da Subscrição")] public string EstadoDaSubscricao { get; set; }
        public bool Ativo { get; set; } = true;
        public string Email { get; set; }

        [DisplayName("Telemóvel")] public string PhoneNumber { get; set; }

        public string Nome { get; set; }

        public string Apelido { get; set; }

        public Localizacao Localizacao { get; set; } = new();
    }
}