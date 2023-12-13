using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks; // Adicione o using para Task
using HabitAqui.Models;
using HabitAqui.Areas.Identity.Pages.Account.Manage;

namespace HabitAqui.Controllers
{
    [Route("Admin")]
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
            var utilizadores = _userManager.Users.Select(u => new DetalhesUtilizador // Use DetalhesUtilizador em vez de IdentityUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Active = u.Active // Adicione Active
                // Adicione outras propriedades do DetalhesUtilizador conforme necessário
            }).ToList();

            return View(utilizadores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/ToggleUserStatus/{id}")]
        public async Task<IActionResult> ToggleUserStatus(string id)
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
                // Obtenha o cabeçalho "Referer" para saber a página anterior
                var referer = Request.Headers["Referer"].ToString();

                // Redirecione de volta para a página anterior
                return Redirect(referer);
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
