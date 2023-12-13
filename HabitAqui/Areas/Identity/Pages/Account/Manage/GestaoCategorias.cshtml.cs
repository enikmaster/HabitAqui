using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

public class GestaoCategoriasModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public GestaoCategoriasModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Categoria> Categorias { get; set; }

    public Dictionary<int, int> TotalHabitacoesPorCategoria { get; set; }

    public void OnGet()
    {
        Categorias = _context.Categorias.ToList();

        // Inicializar o dicionário
        TotalHabitacoesPorCategoria = new Dictionary<int, int>();

        // Para cada categoria, obter o total de habitações e armazenar
        foreach (var categoria in Categorias)
        {
            var totalHabitacoes = _context.Habitacoes
                .Count(h => h.Categorias.Any(c => c.Id == categoria.Id));
            TotalHabitacoesPorCategoria[categoria.Id] = totalHabitacoes;
        }
    }

    public int GetTotalHabitacoesComCategoria(int categoriaId)
    {
        // Verifique se a categoriaId existe no dicionário TotalHabitacoesPorCategoria
        if (TotalHabitacoesPorCategoria.ContainsKey(categoriaId)) return TotalHabitacoesPorCategoria[categoriaId];
        return 0;
    }
}