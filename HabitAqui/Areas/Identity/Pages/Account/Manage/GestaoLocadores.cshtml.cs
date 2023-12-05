using System.ComponentModel;
using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
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

    private readonly LocadorService _locadorService;
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly IUserStore<DetalhesUtilizador> _userStore;

    public GestaoLocadoresModel(
        ApplicationDbContext context,
        UserManager<DetalhesUtilizador> userManager,
        IUserStore<DetalhesUtilizador> userStore)
    {
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
    }

    [BindProperty] public InputModel Input { get; set; }
    [TempData] public string StatusMessage { get; set; }
    public IList<Locador> Locadores { get; set; }
    public IList<DetalhesUtilizador> Gestores { get; set; }

    private async Task LoadAsync(int page, int pageSize)
    {
        Locadores = await _context.Locadores
            .Include(response => response.Administradores)
            .Include(response => response.Habitacoes)
            .ToListAsync();
        Gestores = await _userManager.GetUsersInRoleAsync(Roles.Gestor.ToString());
        Input = new InputModel();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadAsync(1, 10);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return RedirectToPage();

        var locador = await CreateLocador();
        SetUserProperties(locador);
        var result = await _userManager.CreateAsync(locador, Input.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(locador, Roles.Administrador.ToString());
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
        /*var locador = CreateLocador();
        locador.Active = true;
        locador.DataInicioSubscricao = DateTime.Now;
        locador.EstadoDaSubscricao = EstadoSubscricao.Ativo.ToString();
        locador.Nome = Input.Nome;
        locador.Apelido = Input.Apelido;
        locador.UserName = Input.Email;
        locador.Email = Input.Email;
        locador.Nif = Input.Nif;
        locador.PhoneNumber = Input.PhoneNumber;
        locador.Localizacao = Input.Localizacao;
        await _userStore.SetUserNameAsync(locador, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(locador, Input.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(locador, Input.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(locador, Roles.Administrador.ToString());

            var gestor = CreateUser();
            gestor.Active = true;
            gestor.UserName = Input.Email;
            gestor.Nome = Input.Nome;
            gestor.Apelido = Input.Apelido;
            gestor.Email = Input.Email;
            gestor.Nif = Input.Nif;
            gestor.PhoneNumber = Input.PhoneNumber;
            gestor.Localizacao = Input.Localizacao;
            await _userStore.SetUserNameAsync(gestor, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(gestor, Input.Email, CancellationToken.None);

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
        return Page();*/
    }

    public async Task<IActionResult> OnSoftDeleteAsync(int id)
    {
        var locador = await _context.Locadores.FindAsync(id);
        if (locador == null) return NotFound();
        if (locador.Habitacoes != null && locador.Habitacoes.Count > 0)
        {
            StatusMessage = "Não é possível eliminar um locador com habitações associadas.";
            return RedirectToPage();
        }

        locador.Active = false;
        locador.EstadoDaSubscricao = EstadoSubscricao.Cancelado.ToString();
        /* TODO: inativar os gestores associados a este locador */
        _context.Locadores.Update(locador);
        await _context.SaveChangesAsync();
        return RedirectToPage();
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

        //[DisplayName("Estado da Subscrição")] public string EstadoDaSubscricao { get; set; }
        public ICollection<Habitacao> Habitacoes { get; set; } = new List<Habitacao>();

        [DisplayName("Funcionário(s) responsável(eis)")]
        public ICollection<DetalhesUtilizador> Administradores { get; set; } = new List<DetalhesUtilizador>();
    }
}