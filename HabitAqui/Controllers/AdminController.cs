﻿using HabitAqui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Controllers;

[Route("Admin")]
public class AdminController : Controller
{
    private readonly UserManager<DetalhesUtilizador> _userManager;

    public AdminController(UserManager<DetalhesUtilizador> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Admin/ToggleUserStatus/{id}")]
    public async Task<IActionResult> ToggleUserStatus(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        user.Active = !user.Active;

        var updateResult = await _userManager.UpdateAsync(user);

        if (updateResult.Succeeded)
        {
            var referer = Request.Headers["Referer"].ToString();

            return Redirect(referer);
        }

        return View("Error");
    }
}