using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using HabitAqui.Models;
using HabitAqui.Areas.Identity.Pages.Account.Manage;

namespace HabitAqui.Controllers
{
    [Area("Manage")]

    public class AdminController : Controller
    {
        private readonly UserManager<DetalhesUtilizador> _userManager;

        public AdminController(UserManager<DetalhesUtilizador> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Implemente a lógica desejada para a página inicial do Admin (se necessário)
            return View();
        }

        public IActionResult ListaUtilizadores()
        {
            var utilizadores = _userManager.Users.Select(u => new IdentityUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                // Adicione outras propriedades do IdentityUser conforme necessário
            }).ToList();

            return View(utilizadores);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/ToggleUserStatus/{id}")]
        public async Task<IActionResult> ToggleUserStatus(string id, string returnUrl)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Inverta o valor de Active (se for verdadeiro, torne falso, e vice-versa)
            user.Active = !user.Active;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                // Redirecione de volta para a página de onde veio (usando o returnUrl)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                // Caso contrário, redirecione para a ação "ListaUtilizadores"
                return RedirectToAction("ListaUtilizadores");
            }
            else
            {
                // Lidar com erros de atualização, se necessário
                // Pode retornar uma página de erro ou fazer o que for apropriado para o seu aplicativo
                return View("Error");
            }
        }

    
    }

}