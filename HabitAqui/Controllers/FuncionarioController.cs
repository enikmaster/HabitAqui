using HabitAqui.Data;
using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Linq;
using HabitAqui.Services;

namespace HabitAqui.Controllers;

public class FuncionarioController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly LocadorService _locadorService;

    public FuncionarioController(ApplicationDbContext context, UserManager<DetalhesUtilizador> userManager, RoleManager<IdentityRole> roleManager, LocadorService locadorService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _locadorService = locadorService;


    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ListarReservas()
    {
        var reservas = _context.Reservas
            .Include(r => r.Funcionario)
            .Include(r => r.Cliente)
            .Include(r => r.Habitacao)
            .ToList();

        if (reservas == null) return NotFound();

        return View(reservas);
    }

    //GET 
    public IActionResult AdicionarGestor()
    {




        return View("FormAdicionarGestor");
    }

        [HttpPost]
        public async Task<IActionResult> AdicionarGestor(DetalhesUtilizador detalhesUtilizador)
        {
            //Enquanto Gestor pretendo criar um novo utilizador com a role Gestor.
            //  Este novo Gestor fica automaticamente associado 
            //ao Locador que está associado ao gestor que o está a criar.

            var locador = await _locadorService.GetLocador(_userManager.GetUserId(User));

            if(locador == null) return NotFound();

            var user = new DetalhesUtilizador { UserName = detalhesUtilizador.Email, Email = detalhesUtilizador.Email };

            user.Nome = detalhesUtilizador.Nome;
            user.Apelido = detalhesUtilizador.Apelido;
            user.Email = detalhesUtilizador.Email;
            user.Nif = detalhesUtilizador.Nif;
            user.PhoneNumber = detalhesUtilizador.PhoneNumber;
            user.Localizacao = detalhesUtilizador.Localizacao;
            

            var createUserResult = await _userManager.CreateAsync(user, "Test123!");


            if (createUserResult.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Gestor"))
                {
                    return NotFound();
                }
                await _userManager.AddToRoleAsync(user, "Gestor");

            //_context.DetalhesUtilizadores.Add(detalhesUtilizador);

            locador.Administradores.Add(user);
            await _context.SaveChangesAsync();

            // Redireciona para a página de sucesso ou lista de gestores
            return View("FormAdicionarGestor"); // Ou outra view conforme necessário
        }
        else
        {
            // Adicione os erros do Identity ao ModelState para mostrar na view
            foreach (var error in createUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Se algo falhar, retorne à view com os dados que o usuário preencheu para que possam ser corrigidos
        return View("FormAdicionarGestor", detalhesUtilizador);
    }


    //public IActionResult CriarGestor

    //[HttpPost]
    //public async Task<IActionResult> AdicionarGestor2(DetalhesUtilizador detalhesUtilizador)
    //{
    //    var user = new IdentityUser { UserName = detalhesUtilizador.Email, Email = detalhesUtilizador.Email };
    //    var result = await _userManager.CreateAsync(user, "Test123!");


    //    var roleExists = await _roleManager.RoleExistsAsync("Gestor");
    //    var roleResult = await _userManager.AddToRoleAsync(user, "Gestor");



    //    _context.DetalhesUtilizadores.Add(detalhesUtilizador);
    //    await _context.SaveChangesAsync();


    //    return View("Index");

    //}
}







