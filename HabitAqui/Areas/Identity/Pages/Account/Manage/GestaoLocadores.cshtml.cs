using System.ComponentModel;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class GestaoLocadoresModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public GestaoLocadoresModel(
        ApplicationDbContext context,
        UserManager<DetalhesUtilizador> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty] public InputModel Input { get; set; }
    [TempData] public string StatusMessage { get; set; }
    public IList<Locador> Locadores { set; get; }

    private async Task LoadAsync()
    {
        Locadores = await _context.Locadores
            .Include(response => response.Administradores)
            .Include(response => response.Habitacoes)
            .ToListAsync();
        Input = new InputModel();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = new DetalhesUtilizador
        {
            Active = true,
            UserName = Input.Email,
            Nome = Input.Nome,
            Apelido = Input.Apelido,
            Email = Input.Email,
            Nif = Input.Nif,
            PhoneNumber = Input.PhoneNumber,
            Localizacao = Input.Localizacao
        };

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            StatusMessage = "Utilizador criado com sucesso.";
            await _userManager.AddToRoleAsync(user, "Gestor");
        }
        else
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        var locador = new Locador
        {
            Nome = Input.NomeLocador,
            EstadoDaSubscricao = Input.EstadoDaSubscricao,
            Ativo = Input.Ativo,
            Administradores = new List<DetalhesUtilizador> { user },
            Habitacoes = Input.Habitacoes
        };
        _context.Locadores.Add(locador);
        var linhasAlteradas = await _context.SaveChangesAsync();
        StatusMessage = linhasAlteradas == 0 ? "Não foi possível criar o locador." : "Locador criado com sucesso.";
        return Page();
    }

    public class InputModel
    {
        [DisplayName("Nome do Locador")] public string NomeLocador { get; set; }

        [DisplayName("Estado da Subscrição")] public string EstadoDaSubscricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Habitacao> Habitacoes { get; set; } = new List<Habitacao>();

        [DisplayName("Funcionário(s) responsável(eis)")]
        public ICollection<DetalhesUtilizador> Administradores { get; set; } = new List<DetalhesUtilizador>();

        public string Email { get; set; }
        public string Password { get; set; }

        [DisplayName("Telemóvel")] public string PhoneNumber { get; set; }

        public string Nome { get; set; }

        public string Apelido { get; set; }

        public string Nif { get; set; }

        public Localizacao Localizacao { get; set; } = new();
    }
}