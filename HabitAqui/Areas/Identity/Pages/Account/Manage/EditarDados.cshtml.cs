using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

// [Authorize]
public class EditarDados : PageModel
{
    private readonly SignInManager<DetalhesUtilizador> _signInManager;
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public EditarDados(
        UserManager<DetalhesUtilizador> userManager,
        SignInManager<DetalhesUtilizador> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty] public InputModel Input { get; set; }

    [TempData] public string StatusMessage { get; set; }

    private async Task LoadAsync(DetalhesUtilizador user)
    {
        Input = new InputModel
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Nome = user.Nome,
            Apelido = user.Apelido,
            Localizacao = user.Localizacao
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.Localizacao)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return NotFound($"Não foi possível carregar o utilizador com o ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //var user = await _userManager.GetUserAsync(User);
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.Localizacao)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return NotFound($"Não foi possível carregar o utilizador com o ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid) return Page();

        if (user.Email != Input.Email)
            user.Email = Input.Email;
        if (user.PhoneNumber != Input.PhoneNumber)
            user.PhoneNumber = Input.PhoneNumber;
        if (user.Nome != Input.Nome)
            user.Nome = Input.Nome;
        if (user.Apelido != Input.Apelido)
            user.Apelido = Input.Apelido;
        if (user.Localizacao.Morada != Input.Localizacao.Morada)
            user.Localizacao.Morada = Input.Localizacao.Morada;
        if (user.Localizacao.CodigoPostal != Input.Localizacao.CodigoPostal)
            user.Localizacao.CodigoPostal = Input.Localizacao.CodigoPostal;
        if (user.Localizacao.Cidade != Input.Localizacao.Cidade)
            user.Localizacao.Cidade = Input.Localizacao.Cidade;
        if (user.Localizacao.Pais != Input.Localizacao.Pais)
            user.Localizacao.Pais = Input.Localizacao.Pais;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            StatusMessage = "Ocorreu um erro inesperado ao tentar atualizar os dados do utilizador.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Os seus dados foram atualizados";
        return RedirectToPage();
    }

    public class InputModel
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Nome { get; set; }

        public string Apelido { get; set; }

        public Localizacao Localizacao { get; set; } = new();
    }
}