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

    //GET 
    public IActionResult AdicionarFuncionario()
    {




        return View("FormAdicionarFuncionario");
    }


    [HttpPost]
    public async Task<IActionResult> AdicionarFuncionario(DetalhesUtilizador detalhesUtilizador)
    {
        

        var locador = await _locadorService.GetLocadorGestor(_userManager.GetUserId(User));

        if (locador == null) return NotFound();

        var user = new DetalhesUtilizador { UserName = detalhesUtilizador.Email, Email = detalhesUtilizador.Email };

        user.Nome = detalhesUtilizador.Nome;
        user.Apelido = detalhesUtilizador.Apelido;
        user.Email = detalhesUtilizador.Email;
        user.Nif = detalhesUtilizador.Nif;
        user.PhoneNumber = detalhesUtilizador.PhoneNumber;
        user.Localizacao = detalhesUtilizador.Localizacao;
        user.Active = true;


        var createUserResult = await _userManager.CreateAsync(user, "Test123!");


        if (createUserResult.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync("Funcionario"))
            {
                return NotFound();
            }
            await _userManager.AddToRoleAsync(user, "Funcionario");

            //_context.DetalhesUtilizadores.Add(detalhesUtilizador);

            locador.Administradores.Add(user);
            await _context.SaveChangesAsync();




          

            var funcionarios = new List<DetalhesUtilizador>();

            foreach (var admin in locador.Administradores)
            {
                if (await _userManager.IsInRoleAsync(admin, "Funcionario")) //funcionarios não existem aqui, testei com Gestor e funciona
                {
                    funcionarios.Add(admin);
                }
            }

            return View("ListarFuncionarios", funcionarios);
        }
        else
        {
            // Adicione os erros do Identity ao ModelState para mostrar na view
            foreach (var error in createUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("FormAdicionarFuncionario", detalhesUtilizador);
    }



    [HttpPost]
        public async Task<IActionResult> AdicionarGestor(DetalhesUtilizador detalhesUtilizador)
        {
            var locador = await _locadorService.GetLocadorGestor(_userManager.GetUserId(User));

            if(locador == null) return NotFound();

            var user = new DetalhesUtilizador { UserName = detalhesUtilizador.Email, Email = detalhesUtilizador.Email };

            user.Nome = detalhesUtilizador.Nome;
            user.Apelido = detalhesUtilizador.Apelido;
            user.Email = detalhesUtilizador.Email;
            user.Nif = detalhesUtilizador.Nif;
            user.PhoneNumber = detalhesUtilizador.PhoneNumber;
            user.Localizacao = detalhesUtilizador.Localizacao;
            user.Active = true;


        var createUserResult = await _userManager.CreateAsync(user, "Test123!");


            if (createUserResult.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Gestor"))
                {
                    return NotFound();
                }
                await _userManager.AddToRoleAsync(user, "Gestor");


            locador.Administradores.Add(user);
            await _context.SaveChangesAsync();

            return View("Index"); 
        }
        else
        {
            foreach (var error in createUserResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("FormAdicionarGestor", detalhesUtilizador);
    }



    public async Task<IActionResult> ListarFuncionariosAsync()
    {
        var locador = await _locadorService.GetLocadorGestor(_userManager.GetUserId(User));
        if (locador == null)
        {
            return NotFound();
        }

        var funcionarios = new List<DetalhesUtilizador>();

        foreach (var admin in locador.Administradores)
        {
                if (await _userManager.IsInRoleAsync(admin, "Funcionario"))
            {
                funcionarios.Add(admin);
            }
        }

        return View("ListarFuncionarios", funcionarios);
    }

    public async Task<IActionResult> ListarGestoresAsync()
    {
        var locador = await _locadorService.GetLocadorGestor(_userManager.GetUserId(User));
        if (locador == null)
        {
            return NotFound();
        }

        var funcionarios = new List<DetalhesUtilizador>();

        foreach (var admin in locador.Administradores)
        {
            if (await _userManager.IsInRoleAsync(admin, "Gestor")) 
            {
                funcionarios.Add(admin);
            }
        }

        return View("ListarGestores", funcionarios);
    }
}







