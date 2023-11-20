using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class ListarUtilizadoresModel : PageModel
    {
        //injeção de dependencias para aceder ao IdentityUser
        private readonly UserManager<IdentityUser> UserManager;

        public ListarUtilizadoresModel(UserManager<IdentityUser> userManager)
        {
            this.UserManager = userManager;
        }

        public IList<UtilizadorComFuncao> Users { get; set; }
        public class UtilizadorComFuncao
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public IList<string> Roles { get; set; }
            // Adicione mais propriedades conforme necessário
        }
        public void OnGet()
        {
            Users = UserManager.Users.Select(u => new UtilizadorComFuncao
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = UserManager.GetRolesAsync(u).Result.ToList()
            }).ToList();
        }
    }
}