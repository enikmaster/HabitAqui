using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class GestaoCategoriasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GestaoCategoriasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Properties for categories and their corresponding total habitations
        public List<Categoria> Categorias { get; set; }
        public Dictionary<int, int> TotalHabitacoesPorCategoria { get; set; }

        // Sorting properties
        public string CurrentSort { get; set; }
        public string IdSort { get; set; }
        public string NomeSort { get; set; }
        public string TotalHabitacoesSort { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10; // Adjust page size as needed

        public void OnGet(string sortOrder, int? pageNumber)
        {
            CurrentSort = sortOrder;
            IdSort = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            NomeSort = sortOrder == "Nome" ? "nome_desc" : "Nome";
            TotalHabitacoesSort = sortOrder == "TotalHabitacoes" ? "totalHabitacoes_desc" : "TotalHabitacoes";

            CurrentPage = pageNumber ?? 1;
            var totalCategoriesCount = _context.Categorias.Count();
            TotalPages = (int)Math.Ceiling(totalCategoriesCount / (double)PageSize);

            IQueryable<Categoria> categoriaQuery = _context.Categorias.AsQueryable();

            switch (sortOrder)
            {
                case "id_desc":
                    categoriaQuery = categoriaQuery.OrderByDescending(c => c.Id);
                    break;
                case "Nome":
                    categoriaQuery = categoriaQuery.OrderBy(c => c.Nome);
                    break;
                case "nome_desc":
                    categoriaQuery = categoriaQuery.OrderByDescending(c => c.Nome);
                    break;
                case "TotalHabitacoes":
                    categoriaQuery = categoriaQuery.OrderBy(c =>
                        _context.Habitacoes.Count(h => h.Categorias.Any(cat => cat.Id == c.Id)));
                    break;
                case "totalHabitacoes_desc":
                    categoriaQuery = categoriaQuery.OrderByDescending(c =>
                        _context.Habitacoes.Count(h => h.Categorias.Any(cat => cat.Id == c.Id)));
                    break;
                default:
                    categoriaQuery = categoriaQuery.OrderBy(c => c.Id);
                    break;
            }

            categoriaQuery = categoriaQuery.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            Categorias = categoriaQuery.ToList();

            TotalHabitacoesPorCategoria = new Dictionary<int, int>();
            foreach (var categoria in Categorias)
            {
                var totalHabitacoes = _context.Habitacoes
                    .Count(h => h.Categorias.Any(cat => cat.Id == categoria.Id));
                TotalHabitacoesPorCategoria[categoria.Id] = totalHabitacoes;
            }
        }
    }
}
