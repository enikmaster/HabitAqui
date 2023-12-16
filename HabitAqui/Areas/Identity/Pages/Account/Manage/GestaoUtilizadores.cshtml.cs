using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class GestaoUtilizadoresModel : PageModel
    {
        private readonly UserManager<DetalhesUtilizador> UserManager;

        // Sorting properties
        public string CurrentSort { get; set; }
        public string UserNameSort { get; set; }
        public string EmailSort { get; set; }
        public string RoleSort { get; set; }  // Added RoleSort

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;

        public GestaoUtilizadoresModel(UserManager<DetalhesUtilizador> userManager)
        {
            UserManager = userManager;
        }

        public IList<UtilizadorComFuncao> Users { get; set; }

        public async Task OnGetAsync(string sortOrder, int? pageNumber)
        {
            CurrentSort = sortOrder;
            UserNameSort = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";
            RoleSort = sortOrder == "Role" ? "role_desc" : "Role";  // Added RoleSort logic

            CurrentPage = pageNumber ?? 1;

            IQueryable<DetalhesUtilizador> usersQuery = UserManager.Users;
            switch (sortOrder)
            {
                case "username_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.UserName);
                    break;
                case "Email":
                    usersQuery = usersQuery.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.Email);
                    break;
                case "Role":
                    // Sorting by the first role - this might need to be adjusted based on your requirements
                    usersQuery = usersQuery.OrderBy(u => UserManager.GetRolesAsync(u).Result.FirstOrDefault());
                    break;
                case "role_desc":
                    usersQuery = usersQuery.OrderByDescending(u => UserManager.GetRolesAsync(u).Result.FirstOrDefault());
                    break;
                default:
                    usersQuery = usersQuery.OrderBy(u => u.UserName);
                    break;
            }

            int totalUsersCount = await usersQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(totalUsersCount / (double)PageSize);

            var users = await usersQuery
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            Users = users.Select(u => new UtilizadorComFuncao
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
            // Additional properties as needed
        }
    }
}
