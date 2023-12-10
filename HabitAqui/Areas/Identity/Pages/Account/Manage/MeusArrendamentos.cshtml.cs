using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HabitAqui.Data; // Certifique-se de importar o namespace do seu contexto de banco de dados.
using HabitAqui.Models; // Certifique-se de importar o namespace do seu modelo de dados.

namespace HabitAqui.Areas.Identity.Pages.Account.Manage
{
    public class MeusArrendamentosModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MeusArrendamentosModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Reserva> Reservas { get; set; } 

        public void OnGet()
        {
            // Recupere as reservas do banco de dados.
            Reservas = _context.Reservas
                .Include(r => r.RegistoEntregas) 
                .ToList();
        }
    }
}
