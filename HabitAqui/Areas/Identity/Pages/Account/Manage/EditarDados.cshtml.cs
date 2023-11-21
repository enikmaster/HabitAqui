using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HabitAqui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

// [Authorize]
public class EditarDados : PageModel
{
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly SignInManager<DetalhesUtilizador> _signInManager;
    
    public EditarDados(
        UserManager<DetalhesUtilizador> userManager,
        SignInManager<DetalhesUtilizador> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    /// <summary>
    ///
    /// </summary>
    public InputModel Input { get; set; }
    
    [TempData]
    public string StatusMessage { get; set; }
    
    public class InputModel
    {
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Nome { get; set; }
        
        public string Apelido { get; set; }
        
        public string Morada { get; set; }
        
        [DisplayName("Código Postal")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato 0000-000.")]
        public string CodigoPostal { get; set; }
        
        public string Cidade { get; set; }
        
        [DisplayName("País")]
        public string Pais { get; set; }
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Não foi possível carregar o utilizador com o ID '{_userManager.GetUserId(User)}'.");

        Input = new InputModel
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Nome = user.Nome,
            Apelido = user.Apelido,
            Morada = user.Localizacao?.Morada,
            CodigoPostal = user.Localizacao?.CodigoPostal,
            Cidade = user.Localizacao?.Cidade,
            Pais = user.Localizacao?.Pais
        };
        
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Não foi possível carregar o utilizador com o ID '{_userManager.GetUserId(User)}'.");
        
        if (!ModelState.IsValid) 
            return Page();
        
        
        //if(user.Email != Input.Email)
            user.Email = Input.Email;
        //if(user.PhoneNumber != Input.PhoneNumber)
            user.PhoneNumber = Input.PhoneNumber;
        //if(user.Nome != Input.Nome)
            user.Nome = Input.Nome;
        //if(user.Apelido != Input.Apelido)
            user.Apelido = Input.Apelido;
        //if(user.Localizacao.Morada != Input.Morada)
            user.Localizacao.Morada = Input.Morada;
        //if(user.Localizacao.CodigoPostal != Input.CodigoPostal)
            user.Localizacao.CodigoPostal = Input.CodigoPostal;
        //if(user.Localizacao.Cidade != Input.Cidade)
            user.Localizacao.Cidade = Input.Cidade;
        //if(user.Localizacao.Pais != Input.Pais)
            user.Localizacao.Pais = Input.Pais;
        
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
}