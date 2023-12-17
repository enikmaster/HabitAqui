using HabitAqui.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class ApagarCategoriaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; }
        public ApagarCategoriaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            CategoriaId = id;
            CategoriaNome = _context.Categorias.Find(id).Nome;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var categoria = await _context.Categorias.FindAsync(CategoriaId);

            if (categoria != null)
            {
                var isCategoriaUsed = await _context.Habitacoes
                    .AnyAsync(h => h.Categorias != null && h.Categorias.Any(c => c.CategoriaId == CategoriaId));

                if (isCategoriaUsed)
                {
                    ModelState.AddModelError("", "Não é possível apagar a categoria porque está associada a uma ou mais habitações.");
                    return Page(); 
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./GestaoCategorias");
        }
    }
}

