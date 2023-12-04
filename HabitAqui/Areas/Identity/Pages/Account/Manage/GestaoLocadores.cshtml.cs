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

public enum Role
{
    Administrador,
    Gestor
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
    public IList<Locador> Locadores { set; get; }
    public IList<DetalhesUtilizador> Gestores { set; get; }

    private async Task LoadAsync(int page, int pageSize)
    {
        Locadores = await _context.Locadores
            .Include(response => response.Administradores)
            .Include(response => response.Habitacoes)
            .ToListAsync();
        Gestores = await _userManager.GetUsersInRoleAsync("Gestor");
        Input = new InputModel();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadAsync(1, 10);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        DetalhesUtilizador? gestorSelecionado;
        if (!string.IsNullOrEmpty(Input.GestorSelecionadoId))
        {
            gestorSelecionado =
                await _userManager.Users.FirstOrDefaultAsync(g => g.Id.Equals(Input.GestorSelecionadoId));
            if (gestorSelecionado == null) return RedirectToPage();
            Input.Nome = gestorSelecionado.Nome;
            Input.Apelido = gestorSelecionado.Apelido;
            Input.Email = gestorSelecionado.Email;
            Input.Nif = gestorSelecionado.Nif;
            Input.PhoneNumber = gestorSelecionado.PhoneNumber;
            Input.Localizacao = gestorSelecionado.Localizacao;
            Input.Password = gestorSelecionado.PasswordHash;
        }

        if (!ModelState.IsValid) return RedirectToPage();
        var locador = CreateLocador();
        locador.Active = true;
        locador.DataInicioSubscricao = DateTime.Now;
        locador.EstadoDaSubscricao = EstadoSubscricao.Ativo.ToString();
        locador.Nome = Input.NomeLocador;
        locador.Apelido = Input.ApelidoLocador;
        locador.UserName = Input.EmailLocador;
        locador.Email = Input.EmailLocador;
        locador.Nif = Input.NifLocador;
        locador.PhoneNumber = Input.PhoneNumberLocador;
        locador.Localizacao = Input.LocalizacaoLocador;
        await _userStore.SetUserNameAsync(locador, Input.EmailLocador, CancellationToken.None);
        await _emailStore.SetEmailAsync(locador, Input.EmailLocador, CancellationToken.None);

        var result = await _userManager.CreateAsync(locador, Input.PasswordLocador);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(locador, Role.Administrador.ToString());
            if (!string.IsNullOrEmpty(Input.GestorSelecionadoId))
            {
                var gestor = Gestores.FirstOrDefault(g => g.Id.Equals(Input.GestorSelecionadoId));
                locador.Administradores.Add(gestor!);
                await _context.SaveChangesAsync();
                StatusMessage = "Locador criado com sucesso.";
                return RedirectToPage();
            }
            else
            {
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
                    await _userManager.AddToRoleAsync(gestor, Role.Gestor.ToString());
                    locador.Administradores.Add(gestor);
                    await _context.SaveChangesAsync();
                    StatusMessage = "Locador e gestor criados com sucesso.";
                    return RedirectToPage();
                }

                foreach (var error in resultGestor.Errors) ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
        /*if (!ModelState.IsValid) return Page();

        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        user.Active = true;
        user.UserName = Input.Email;
        user.Nome = Input.Nome;
        user.Apelido = Input.Apelido;
        user.Email = Input.Email;
        user.Nif = Input.Nif;
        user.PhoneNumber = Input.PhoneNumber;
        user.Localizacao = Input.Localizacao;

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
            DataInicioSubscricao = DateTime.Now,
            EstadoDaSubscricao = EstadoSubscricao.Ativo.ToString(),
            Administradores = new List<DetalhesUtilizador> { user },
            Habitacoes = Input.Habitacoes
        };
        _context.Locadores.Add(locador);
        var linhasAlteradas = await _context.SaveChangesAsync();
        //await _signInManager.RefreshSignInAsync(user);
        StatusMessage = linhasAlteradas == 0 ? "Não foi possível criar o locador." : "Locador criado com sucesso.";*/
    }

    public async Task<IActionResult> OnSoftDeleteAsync(int id)
    {
        var locador = await _context.Locadores.FindAsync(id);
        if (locador == null) return NotFound();
        locador.Active = false;
        _context.Locadores.Update(locador);
        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    private DetalhesUtilizador CreateUser()
    {
        try
        {
            return Activator.CreateInstance<DetalhesUtilizador>();
        }
        catch
        {
            throw new InvalidOperationException(
                $"Não foi possível criar uma instância de '{nameof(DetalhesUtilizador)}'. " +
                $"Certifique-se que '{nameof(DetalhesUtilizador)}' não é uma classe abstrata e tem um construtor sem parâmetros.");
        }
    }

    private Locador CreateLocador()
    {
        try
        {
            return Activator.CreateInstance<Locador>();
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
        public string GestorSelecionadoId { get; set; } = string.Empty;
        [DisplayName("Nome")] public string NomeLocador { get; set; }
        public string Nome { get; set; }
        [DisplayName("Apelido")] public string ApelidoLocador { get; set; }
        public string Apelido { get; set; }
        [DisplayName("Email")] public string EmailLocador { get; set; }
        public string Email { get; set; }
        [DisplayName("Password")] public string PasswordLocador { get; set; }
        public string Password { get; set; }
        [DisplayName("Telemóvel")] public string PhoneNumberLocador { get; set; }
        [DisplayName("Telemóvel")] public string PhoneNumber { get; set; }
        [DisplayName("Nif")] public string NifLocador { get; set; }
        public string Nif { get; set; }
        public Localizacao LocalizacaoLocador { get; set; } = new();
        public Localizacao Localizacao { get; set; } = new();
        [DisplayName("Estado da Subscrição")] public string EstadoDaSubscricao { get; set; }
        public ICollection<Habitacao> Habitacoes { get; set; } = new List<Habitacao>();

        [DisplayName("Funcionário(s) responsável(eis)")]
        public ICollection<DetalhesUtilizador> Administradores { get; set; } = new List<DetalhesUtilizador>();
    }
}