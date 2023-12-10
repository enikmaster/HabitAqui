using HabitAqui.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class ApagarCategoriaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public int CategoriaId { get; set; }

        public ApagarCategoriaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            CategoriaId = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var categoria = await _context.Categorias.FindAsync(CategoriaId);

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./GestaoCategorias");
        }
    }
}
