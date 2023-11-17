using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Controllers;

//podemos alterar para [Authorize] e só permite se estivermos logados
[Authorize(Roles = "Administrador, Gestor, Funcionario, Cliente")]
public class HabitacaoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<DetalhesHabitacao> detalhesHabitacao;

    public HabitacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detalhes()
    {
        return View();
    }
}