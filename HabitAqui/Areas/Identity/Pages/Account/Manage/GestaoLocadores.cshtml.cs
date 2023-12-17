using System.ComponentModel;
using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public enum EstadoSubscricao
{
    Ativo,
    Suspenso,
    Cancelado
}

public class GestaoLocadoresModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IUserEmailStore<DetalhesUtilizador> _emailStore;
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly IUserStore<DetalhesUtilizador> _userStore;

    public GestaoLocadoresModel(ApplicationDbContext context, UserManager<DetalhesUtilizador> userManager,
        IUserStore<DetalhesUtilizador> userStore)
    {
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        Input = new InputModel(); // Initialize Input here
    }

    // Sorting properties
    public string EstadoSubscricaoSort { get; set; }
    public string IdSort { get; set; }
    public string NomeSort { get; set; }
    public string EstadoSort { get; set; }
    public string TotalHabitacoesSort { get; set; }
    public string AdministradoresSort { get; set; }

    // Pagination properties
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    [BindProperty] public InputModel Input { get; set; }

    [TempData] public string StatusMessage { get; set; }

    public IList<Locador> Locadores { get; set; }
    public IList<DetalhesUtilizador> Gestores { get; set; }

    private async Task LoadAsync(int page, int pageSize, string sortOrder)
    {
        // Initialize sort properties
        EstadoSubscricaoSort = sortOrder == "EstadoSubscricao" ? "estado_subscricao_desc" : "EstadoSubscricao";
        IdSort = sortOrder == "Id" ? "id_desc" : "Id";
        NomeSort = sortOrder == "Nome" ? "nome_desc" : "Nome";
        EstadoSort = sortOrder == "Estado" ? "estado_desc" : "Estado";
        TotalHabitacoesSort = sortOrder == "TotalHabitacoes" ? "total_habitacoes_desc" : "TotalHabitacoes";
        AdministradoresSort = sortOrder == "Administradores" ? "administradores_desc" : "Administradores";

        var locadoresQuery = _context.Locadores.AsQueryable();

        // Apply sorting
        switch (sortOrder)
        {
            case "estado_subscricao_desc":
                locadoresQuery = locadoresQuery.OrderByDescending(l => l.EstadoDaSubscricao);
                break;
            case "Id":
                locadoresQuery = locadoresQuery.OrderBy(l => l.Id);
                break;
            case "id_desc":
                locadoresQuery = locadoresQuery.OrderByDescending(l => l.Id);
                break;
            // Add more cases for other sorting criteria
            default:
                locadoresQuery = locadoresQuery.OrderBy(l => l.Nome);
                break;
        }

        TotalPages = (int)Math.Ceiling(await locadoresQuery.CountAsync() / (double)pageSize);
        CurrentPage = page;

        Locadores = await locadoresQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(l => l.Administradores)
            .Include(l => l.Habitacoes)
            .ToListAsync();
    }


    public async Task<IActionResult> OnGetAsync(int currentPage = 1, string sortOrder = "")
    {
        await LoadAsync(currentPage, PageSize, sortOrder);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int currentPage = 1)
    {
        if (!ModelState.IsValid)
        {
            await LoadAsync(currentPage, PageSize, "");
            return Page();
        }

        var locador = await CreateLocador();
        SetUserProperties(locador);
        var result = await _userManager.CreateAsync(locador, Input.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(locador, Roles.Gestor.ToString());
            var gestor = await CreateGestor(Roles.Gestor.ToString());
            SetUserProperties(gestor);
            var resultGestor = await _userManager.CreateAsync(gestor, Input.Password);
            if (resultGestor.Succeeded)
            {
                await _userManager.AddToRoleAsync(gestor, Roles.Gestor.ToString());
                locador.Administradores.Add(gestor);
                await _context.SaveChangesAsync();
                StatusMessage = "Locador e gestor criados com sucesso.";
                return RedirectToPage();
            }

            foreach (var error in resultGestor.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    private void SetUserProperties(DetalhesUtilizador user)
    {
        user.Nome = Input.Nome;
        user.Apelido = Input.Apelido;
        user.Email = Input.Email;
        user.Nif = Input.Nif;
        user.PhoneNumber = Input.PhoneNumber;
        user.Localizacao = Input.Localizacao;
    }

    private async Task<DetalhesUtilizador> CreateGestor(string role)
    {
        try
        {
            var user = Activator.CreateInstance<DetalhesUtilizador>();
            user.Active = true;
            user.UserName = $"{role.ToLower()}_{Input.Email}";
            user.Email = Input.Email;
            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            return user;
        }
        catch
        {
            throw new InvalidOperationException(
                $"Não foi possível criar uma instância de '{nameof(DetalhesUtilizador)}'. " +
                $"Certifique-se que '{nameof(DetalhesUtilizador)}' não é uma classe abstrata e tem um construtor sem parâmetros.");
        }
    }

    private async Task<Locador> CreateLocador()
    {
        try
        {
            var user = Activator.CreateInstance<Locador>();
            user.Active = true;
            user.UserName = Input.Email;
            user.Email = Input.Email;
            user.DataInicioSubscricao = DateTime.Now;
            user.EstadoDaSubscricao = EstadoSubscricao.Ativo.ToString();
            user.Administradores = Input.Administradores;
            user.Habitacoes = Input.Habitacoes;
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            return user;
        }
        catch
        {
            throw new InvalidOperationException($"Não foi possível criar uma instância de '{nameof(Locador)}'. " +
                                                $"Certifique-se que '{nameof(Locador)}' não é uma classe abstrata e tem um construtor sem parâmetros.");
        }
    }

    private IUserEmailStore<DetalhesUtilizador> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("O UI padrão requer um armazenamento de utilizador com suporte por email.");
        return (IUserEmailStore<DetalhesUtilizador>)_userStore;
    }

    public class InputModel
    {
        [DisplayName("Nome")] public string Nome { get; set; }
        [DisplayName("Apelido")] public string Apelido { get; set; }
        [DisplayName("Email")] public string Email { get; set; }
        [DisplayName("Password")] public string Password { get; set; }
        [DisplayName("Telemóvel")] public string PhoneNumber { get; set; }
        [DisplayName("Nif")] public string Nif { get; set; }
        public Localizacao Localizacao { get; set; } = new();
        public ICollection<Habitacao> Habitacoes { get; set; } = new List<Habitacao>();

        [DisplayName("Funcionários")]
        public ICollection<DetalhesUtilizador> Administradores { get; set; } = new List<DetalhesUtilizador>();
    }
}