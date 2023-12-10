using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HabitAqui.Models;
using System.Threading.Tasks;
using HabitAqui.Data;
using Microsoft.EntityFrameworkCore;


public class EditarCategoriaModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public Categoria Categoria { get; set; }

    public EditarCategoriaModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Categoria = await _context.Categorias.FindAsync(id);

        if (Categoria == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExists(Categoria.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./GestaoCategorias");
    }

    private bool CategoriaExists(int id)
    {
        return _context.Categorias.Any(e => e.Id == id);
    }
}
