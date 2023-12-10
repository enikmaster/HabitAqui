using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class AdicionarCategoriaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Categoria NovaCategoria { get; set; }

        public AdicionarCategoriaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categorias.Add(NovaCategoria);
            await _context.SaveChangesAsync();

            return RedirectToPage("./GestaoCategorias");
        }
    }
}
