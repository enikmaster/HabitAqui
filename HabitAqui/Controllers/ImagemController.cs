using HabitAqui.Data;
using HabitAqui.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitAqui.Controllers;

public class ImagemController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ImagemService _imagemService;

    public ImagemController(ApplicationDbContext context, ImagemService imagemService)
    {
        _context = context;
        _imagemService = imagemService;
    }

    [HttpPost]
    public async Task<IActionResult> RemoverImagem([FromBody] string imagemPath, int habitacaoId)
    {
        try
        {
            var imagem = _context.Imagens.SingleOrDefault(i => i.Path == imagemPath);
            if (imagem == null) return NotFound();
            var novaListaImagens = await _imagemService.RemoverImagem(imagem, habitacaoId);
            return Json(novaListaImagens);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro interno: {e.Message}");
        }
    }
}