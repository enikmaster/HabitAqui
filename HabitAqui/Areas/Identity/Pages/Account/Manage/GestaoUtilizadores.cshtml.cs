using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class GestaoUtilizadoresModel : PageModel
{
    //injeção de dependências para aceder ao IdentityUser
    private readonly UserManager<DetalhesUtilizador> UserManager;

    public GestaoUtilizadoresModel(UserManager<DetalhesUtilizador> userManager)
    {
        UserManager = userManager;
    }

    public IList<UtilizadorComFuncao> Users { get; set; }

    public void OnGet()
    {
        Users = UserManager.Users.Select(u => new UtilizadorComFuncao
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Roles = UserManager.GetRolesAsync(u).Result.ToList(),
            Active = u.Active
        }).ToList();
    }

    public class UtilizadorComFuncao
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Boolean Active { get; set; }
        public IList<string> Roles { get; set; }
        // Adicione mais propriedades conforme necessário
    }
}