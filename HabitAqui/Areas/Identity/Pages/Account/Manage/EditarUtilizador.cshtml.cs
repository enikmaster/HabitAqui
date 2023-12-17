using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class EditarUtilizadorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<DetalhesUtilizador> _userManager;

        [BindProperty]
        public DetalhesUtilizador DetalhesUtilizador { get; set; }

        public EditarUtilizadorModel(ApplicationDbContext context, UserManager<DetalhesUtilizador> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("ID Utilizador não fornecido");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Utilizador com ID '{userId}' não encontrado.");
            }

            DetalhesUtilizador = user;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var user = await _userManager.FindByIdAsync(DetalhesUtilizador.Id);

            if (user == null)
            {
                return NotFound($"Utilizador com ID '{DetalhesUtilizador.Id}' não encontrado.");
            }

            try
            {
            user.Nome = DetalhesUtilizador.Nome;
            user.Apelido = DetalhesUtilizador.Apelido;
            user.Nif = DetalhesUtilizador.Nif;
            user.PhoneNumber = DetalhesUtilizador.PhoneNumber;
            user.Active = DetalhesUtilizador.Active;
            user.Localizacao = DetalhesUtilizador.Localizacao;
            user.Localizacao.Morada = DetalhesUtilizador.Localizacao.Morada;
            user.Localizacao.CodigoPostal = DetalhesUtilizador.Localizacao.CodigoPostal;
            user.Localizacao.Pais = DetalhesUtilizador.Localizacao.Pais;
            user.Localizacao.Cidade = DetalhesUtilizador.Localizacao.Cidade;
          

            var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar o usuário.");
                return Page();
            }




           

            return RedirectToPage("./Index"); // Substitua pelo caminho da página desejada
        }
    }
}
