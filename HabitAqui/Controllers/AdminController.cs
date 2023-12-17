using HabitAqui.Data;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers;

[Route("Admin")]
public class AdminController : Controller
{
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly LocadorService _locadorService;

    public AdminController(UserManager<DetalhesUtilizador> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, LocadorService locadorService)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
        _locadorService = locadorService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Admin/ToggleUserStatus/{id}")]
    public async Task<IActionResult> ToggleUserStatus(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null) return NotFound();
        var userEscolhido = await _context.Users.FindAsync(id);

        if (userEscolhido.Active)
        {
            if (await _userManager.IsInRoleAsync(user, "Gestor") || await _userManager.IsInRoleAsync(user, "Funcionario") || userEscolhido.Apelido.ToLower() == "lda") //se é gestor ou funcionario ou locador
            {

                if (userEscolhido.Apelido.ToLower() == "lda") // se for locador
                {
                    var countaHabitacoes = _context.Habitacoes.Where(h => h.LocadorId == userEscolhido.Id && h.Active).Count();
                    if (countaHabitacoes > 0)
                    {

                        TempData["WarningMessage"] = "Não é possível desativar o Locador porque existem habitações ativas associadas";
                        var refereres = Request.Headers["Referer"].ToString();

                        return Redirect(refereres);
                    }
                    else
                    {
                        var administradores = _context.Locadores
                                                                .Include(l => l.Administradores)
                                                                .Where(l => l.Id == userEscolhido.Id)
                                                                .ToList();

                        var administradoresLista = administradores[0].Administradores.ToList();

                        foreach (var administrador in administradoresLista)
                        {
                            administrador.Active = false;
                        }
                        await _context.SaveChangesAsync();
                        userEscolhido.Active = false;
                        await _context.SaveChangesAsync();

                    }
                     var refereress = Request.Headers["Referer"].ToString();
                        return Redirect(refereress);
                }
                else // se for gestor ou funcionario verificamos o estado das reservas
                {
                    var reservas = await _context.Reservas
                                           .Where(r => r.FuncionarioId == userEscolhido.Id && (r.Estado != EstadoReserva.Concluido || r.Estado != EstadoReserva.Rejeitado))
                                           .ToListAsync();

                    if (reservas.Count > 0) TempData["WarningMessage"] = "Não é possível desativar o utilizador porque existem habitações com reservas ativas/pendentes associadas";
                    else
                    { // se não existirem reservas ativas/pendentes
                        userEscolhido.Active = false;
                        await _context.SaveChangesAsync();
                    }
                }
                var referer = Request.Headers["Referer"].ToString();
                return Redirect(referer);
            }
            else
            {
                user.Active = !user.Active;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    var referer = Request.Headers["Referer"].ToString();

                    return Redirect(referer);
                }
            }
        }
        else
        {
            if((await _userManager.IsInRoleAsync(user, "Gestor") || await _userManager.IsInRoleAsync(user, "Funcionario")) && userEscolhido.Apelido.ToLower() != "lda")
            {
               var getLocador = _context.Locadores
                                        .Include(l => l.Administradores)
                                        .Where(l => l.Administradores.Any(a => a.Id == userEscolhido.Id))
                                        .ToList();
               if(getLocador.FirstOrDefault().Active)
                {
                    userEscolhido.Active = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["WarningMessage"] = "Não é possível activar o utilizador porque o seu locador está desativado";
                    var refereres = Request.Headers["Referer"].ToString();

                    return Redirect(refereres);
                }
               
            }
            else if (userEscolhido.Apelido.ToLower() == "lda")
            {
                userEscolhido.Active = true;
                await _context.SaveChangesAsync();

                
            }
            else {
                userEscolhido.Active = true;
                await _context.SaveChangesAsync();
            }
            
            var referer = Request.Headers["Referer"].ToString();

            return Redirect(referer);
        }

        return View("Error");
    }
}