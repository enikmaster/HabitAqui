using System.ComponentModel.DataAnnotations;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<DetalhesUtilizador> _signInManager;
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public IndexModel(
        UserManager<DetalhesUtilizador> userManager,
        SignInManager<DetalhesUtilizador> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new();

    private async Task LoadAsync(DetalhesUtilizador user)
    {
        //var userName = await _userManager.GetUserNameAsync(user);
        //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        //Username = userName;

        Input = new InputModel
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Nome = user.Nome,
            Apelido = user.Apelido,
            Nif = user.Nif,
            Localizacao = user.Localizacao
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        //var user = await _userManager.GetUserAsync(User);
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.Localizacao)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Nome { get; set; }

        public string Apelido { get; set; }

        [EmailAddress] public string Email { get; set; }

        [Phone] [Display(Name = "Telemóvel")] public string PhoneNumber { get; set; }

        public string Nif { get; set; }
        public Localizacao Localizacao { get; set; } = new();
    }
}