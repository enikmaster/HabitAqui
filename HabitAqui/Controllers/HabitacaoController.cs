using System.Security.Claims;
using HabitAqui.Data;
using HabitAqui.Dtos;
using HabitAqui.Dtos.Avaliacao;
using HabitAqui.Dtos.Habitacao;
using HabitAqui.Models;
using HabitAqui.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Controllers;

public class HabitacaoController : Controller
{
    private readonly CategoriaService _categoriaService;
    private readonly ApplicationDbContext _context;
    private readonly HabitacaoService _habitacaoService;
    private readonly LocadorService _locadorService;
    private readonly UserManager<DetalhesUtilizador> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HabitacaoController(ApplicationDbContext context,
        HabitacaoService habitacaoService,
        CategoriaService categoriaService,
        LocadorService locadorService,
        UserManager<DetalhesUtilizador> userManager,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _habitacaoService = habitacaoService;
        _categoriaService = categoriaService;
        _locadorService = locadorService;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }
    // DONE: Index
    // DONE: Create
    // TODO: Update/Edit
    // TODO: Delete

    // GET: Habitacao
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        // Fetch the categories for filtering or display purposes
        var categories = await _context.Categorias.Select(c => c.Nome).ToListAsync();
        ViewBag.Categories = categories;

        // Define the base query for Habitacoes
        var query = new List<Habitacao>();

        // If the user is not in a specific role, filter the active Habitacoes
        if (!User.IsInRole(Roles.Funcionario.ToString()) && !User.IsInRole(Roles.Gestor.ToString()))
            query = await _habitacaoService.GetAllActiveHabitacoes(); // Assuming there is an IsActive property


        // Apply the pagination logic
        var totalRecords = query.Count;
        var results = query
            .OrderBy(h => h.Id) // Or order by another appropriate property
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        // Pass the pagination metadata to the view via ViewBag
        ViewBag.TotalRecords = totalRecords;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.PageSize =
            pageSize; // You can also pass this to the view if you want to allow changing the page size dynamically

        return View(results);
    }


    public async Task<IActionResult> Search(string search, string sortOrder, int page = 1, int pageSize = 10)
    {
        ViewData["TitleSearch"] = "Resultados da sua pesquisa: " + search;
        // Sort parameters
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["PriceSortParm"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
        ViewData["RatingSortParm"] = sortOrder == "rating_asc" ? "rating_desc" : "rating_asc";

        var query = _context.Habitacoes
            .Where(h => (h.Active && (string.IsNullOrEmpty(search)
                                      || EF.Functions.Like(h.Locador.Nome, $"%{search}%")
                                      || EF.Functions.Like(h.DetalhesHabitacao.Localizacao.Cidade, $"%{search}%")
                                      || EF.Functions.Like(h.DetalhesHabitacao.Localizacao.CodigoPostal,
                                          $"%{search}%")))
                        || h.Categorias.Any(c => EF.Functions.Like(c.Categoria.Nome, $"%{search}%"))
                        || EF.Functions.Like(h.DetalhesHabitacao.Localizacao.Pais, $"%{search}%"))
            .Include(h => h.DetalhesHabitacao)
            .ThenInclude(h => h.Localizacao)
            .Include(h => h.Avaliacoes)
            .Include(h => h.Categorias)
            .Include(h => h.Locador)
            .Include(h => h.Reservas)
            .Include(h => h.Imagens)
            .AsNoTracking();

        switch (sortOrder)
        {
            case "name_desc":
                query = query.OrderByDescending(h => h.DetalhesHabitacao.Nome);
                break;
            case "price_asc":
                query = query.OrderBy(h => h.DetalhesHabitacao.PrecoPorNoite);
                break;
            case "price_desc":
                query = query.OrderByDescending(h => h.DetalhesHabitacao.PrecoPorNoite);
                break;
            case "rating_asc":
                query = query.OrderBy(h =>
                    h.Avaliacoes.Average(a => (double?)a.Nota) ?? 0); // Assumes 'Rating' is a column in 'Avaliacoes'
                break;
            case "rating_desc":
                query = query.OrderByDescending(h => h.Avaliacoes.Average(a => (double?)a.Nota) ?? 0);
                break;
            default: // Default to name ascending if no sort order is specified
                query = query.OrderBy(h => h.DetalhesHabitacao.Nome);
                break;
        }

        var totalRecords = await query.CountAsync();
        var results = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.TotalRecords = totalRecords;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = search; // Pass the search term back to the view
        ViewBag.CurrentSort = sortOrder; // Pass the current sort order back to the view

        return View(results);
    }


    // GET: Habitacao/Details/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null) return NotFound();

        var habitacao = await _context.Habitacoes
            .Include(i => i.Imagens)
            .Include(d => d.DetalhesHabitacao)
            .ThenInclude(l => l.Localizacao)
            .Include(h => h.Avaliacoes)! // Carrega as avaliações relacionadas
            .ThenInclude(a => a.Cliente) // Inclui informações do cliente que fez a avaliação
            .FirstOrDefaultAsync(m => m.Id == id);

        if (habitacao == null) return NotFound();

        return View(habitacao);
    }

    // GET: Habitacao/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categorias = await _categoriaService.GetAllActive();
        ViewBag.Categorias = (categorias.Any() ? categorias : null)!;
        return View();
    }

    // POST: Habitacao/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HabitacaoDto habitacaoDto,
        [FromForm(Name = "Imagens")] List<IFormFile> imagens)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var locador = await _locadorService.GetLocador(_userManager.GetUserId(User));
                var habitacao = new Habitacao
                {
                    Active = true,
                    LocadorId = locador.Id,
                    DetalhesHabitacao = new DetalhesHabitacao
                    {
                        Nome = habitacaoDto.Nome,
                        Descricao = habitacaoDto.Descricao,
                        PrecoPorNoite = habitacaoDto.PrecoPorNoite,
                        Area = habitacaoDto.Area,
                        Localizacao = new Localizacao
                        {
                            Morada = habitacaoDto.Morada,
                            CodigoPostal = habitacaoDto.CodigoPostal,
                            Cidade = habitacaoDto.Cidade,
                            Pais = habitacaoDto.Pais
                        }
                    },
                    Imagens = new List<Imagem>(),
                    Categorias = new List<HabitacaoCategoria>()
                };
                await _habitacaoService.CreateHabitacao(habitacao);
                if (habitacaoDto.CategoriasId != null)
                    foreach (var categoriaId in habitacaoDto.CategoriasId)
                    {
                        var categoria = await _categoriaService.GetCategoria(categoriaId);
                        if (categoria != null)
                        {
                            var habitacaoCategoria = new HabitacaoCategoria
                            {
                                HabitacaoId = habitacao.Id,
                                CategoriaId = categoria.Id
                            };
                            _context.HabitacoesCategorias.Add(habitacaoCategoria);
                            //await _context.SaveChangesAsync();
                            habitacao.Categorias.Add(habitacaoCategoria);
                        }
                    }

                if (imagens.Count <= 0) return RedirectToAction(nameof(Index));
                foreach (var imagem in imagens)
                {
                    if (imagem.ContentType != "image/jpeg" && imagem.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Imagens", "Apenas são permitidos ficheiros JPG ou PNG.");
                        return View(habitacaoDto);
                    }

                    var uniqueFileName = Guid.NewGuid() + "." + imagem.ContentType.Split('/')[1];
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "imgs", "habitacoes",
                        habitacao.Id.ToString(),
                        uniqueFileName);
                    var imageDirectory = Path.GetDirectoryName(imagePath);
                    if (imageDirectory != null)
                        if (!Directory.Exists(imageDirectory))
                            Directory.CreateDirectory(imageDirectory);

                    await using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    habitacaoDto.Imagens.Add(uniqueFileName);

                    var novaImagem = new Imagem
                    {
                        Path = uniqueFileName,
                        HabitacaoId = habitacao.Id
                    };
                    _context.Imagens.Add(novaImagem);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Não foi possível guardar as alterações. " +
                                         "Tente novamente e, se o problema persistir, " +
                                         "consulte o administrador do sistema.");
        }

        return View(habitacaoDto);
    }

    // GET: Habitacao/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    var editarHabitacaoDto = new EditarHabitacaoDto();
    //    if (id == null || _context.Habitacoes == null) return NotFound();
    //    var habitacao = await _habitacaoService.GetHabitacao(id);
    //    if (habitacao == null) return NotFound();
    //    editarHabitacaoDto.Id = habitacao.Id;
    //    editarHabitacaoDto.Nome = habitacao.DetalhesHabitacao.Nome;
    //    editarHabitacaoDto.Descricao = habitacao.DetalhesHabitacao.Descricao;
    //    editarHabitacaoDto.PrecoPorNoite = habitacao.DetalhesHabitacao.PrecoPorNoite;
    //    editarHabitacaoDto.Area = habitacao.DetalhesHabitacao.Area;
    //    editarHabitacaoDto.Morada = habitacao.DetalhesHabitacao.Localizacao.Morada;
    //    editarHabitacaoDto.CodigoPostal = habitacao.DetalhesHabitacao.Localizacao.CodigoPostal;
    //    editarHabitacaoDto.Cidade = habitacao.DetalhesHabitacao.Localizacao.Cidade;
    //    editarHabitacaoDto.Pais = habitacao.DetalhesHabitacao.Localizacao.Pais;
    //    editarHabitacaoDto.Imagens = habitacao.Imagens.Select(p => p.Path).ToList();
    //    editarHabitacaoDto.ImagensId = habitacao.Imagens.Select(i => i.Id).ToList();
    //    editarHabitacaoDto.CategoriasId = habitacao.Categorias.Select(c => c.CategoriaId).ToList();

    //    var categorias = await _categoriaService.GetAllActive();
    //    ViewBag.Categorias = (categorias.Any() ? categorias : null)!;
    //    return View(editarHabitacaoDto);
    //}

    // POST: Habitacao/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id")] EditarHabitacaoDto habitacaoDto)
    //{
    //    if (id != habitacaoDto.Id) return NotFound();

    //    if (!ModelState.IsValid) return View(habitacaoDto);
    //    try
    //    {
    //        _context.Update(habitacaoDto);
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if ((_context.Habitacoes?.Any(e => e.Id == habitacaoDto.Id)).GetValueOrDefault())
    //            return NotFound();
    //        throw;
    //    }

    //    return RedirectToAction(nameof(Index));
    //}

    // GET: Habitacao/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var habitacao = await _context.Habitacoes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (habitacao == null) return NotFound();

        return View(habitacao);
    }

    // POST: Habitacao/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Habitacoes == null) return Problem("Entity set 'ApplicationDbContext.Habitacoes' is null.");
        var habitacao = await _context.Habitacoes.FindAsync(id);
        if (habitacao != null) _context.Habitacoes.Remove(habitacao);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /*private bool HabitacaoExists(int id)
    {
        return (_context.Habitacoes?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/


    private bool VerificarCondicõesParaAvaliacao(Habitacao habitacao, DetalhesUtilizador utilizador)
    {
        var utilizadorJaAvaliouHabitacao =
            _context.Avaliacoes.Any(a => a.HabitacaoId == habitacao.Id && a.Cliente.Id == utilizador.Id);

        var teveArrendamentoAnterior =
            habitacao.Reservas != null && habitacao.Reservas.Any(r =>
                r.Cliente == utilizador &&
                (r.Estado == EstadoReserva.Aceite || (r.RegistoEntregas != null &&
                                                      r.RegistoEntregas.Any(re =>
                                                          re.TipoTransacao == TipoTransacao.Devolucao))));

        return !utilizadorJaAvaliouHabitacao && teveArrendamentoAnterior;
    }

    // GET: Habitacao/Avaliar/5
    public async Task<IActionResult> Avaliar(int? id)
    {
        if (id == null) return NotFound();
        var habitacao = await _habitacaoService.GetHabitacao(id.Value);
        if (habitacao == null) return NotFound();
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return BadRequest();
        var utilizadorPodeAvaliar = VerificarCondicõesParaAvaliacao(habitacao, currentUser);
        if (!utilizadorPodeAvaliar)
        {
            //return BadRequest("O utilizador não pode avaliar esta habitação com base nas condições especificadas.");
            ViewBag.ErroAvaliacao =
                "O utilizador não pode avaliar esta habitação com base nas condições especificadas.";
            return View("ErroAvaliacao");
        }

        //return BadRequest("O utilizador não pode avaliar esta habitação com base nas condições especificadas.");
        var avaliacao = new Avaliacao { HabitacaoId = habitacao.Id };
        return View("Avaliacao", avaliacao);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] // TODO: ESPECIFICAR A ROLE CLIENTE <-  Roles="Cliente"
    public async Task<IActionResult> Avaliar(int id, AvaliacaoDto avaliacaoDto)
    {
        if (ModelState.IsValid)
        {
            var reserva = _context.Reservas.FirstOrDefault(r => r.Id == id);
            if (reserva == null || reserva.Estado != EstadoReserva.Aceite)
                return BadRequest("A reserva não está disponivel para avaliação");
            var habitacao = await _habitacaoService.GetHabitacao(avaliacaoDto.HabitacaoId);
            if (habitacao == null) return BadRequest();

            // TODO: verificar se  ->>>> o lease dele já tem avaliação ? então bad request
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return BadRequest();
            var novaAvalicaoObj = new Avaliacao
            {
                Cliente = currentUser,
                Comentario = avaliacaoDto.Comentario,
                HabitacaoId = habitacao.Id,
                Nota = avaliacaoDto.Nota
            };

            var novaAvalicao = _context.Avaliacoes.Add(novaAvalicaoObj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalhes), new { id = avaliacaoDto.HabitacaoId });
        }

        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros) Console.WriteLine(erro.ErrorMessage); // Logar os erros
        }

        //return View("Avaliacao", avaliacao); TODO: verificar esta situação
        return View("Avaliacao");
    }

    public async Task<IActionResult> Avaliacoes([FromRoute] int id, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var habitacao = await _habitacaoService.GetHabitacaoReservasPaginadas(id, page, pageSize);
        if (habitacao == null) return NotFound();
        var resultados = new PaginatedDto<Habitacao>
        {
            page = page,
            pageSize = pageSize,
            Value = habitacao,
            Id = id
        };
        return View(resultados);
    }

    // GET: Habitacao/MinhasAvaliacoes
    [Authorize]
    public async Task<IActionResult> MinhasAvaliacoes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var minhasAvaliacoes = await _context.Avaliacoes
            .Include(a => a.Habitacao)
            .ThenInclude(h => h.DetalhesHabitacao)
            .Where(a => a.Cliente.Id == userId) // Altere de a.Cliente para a.Cliente.Id
            .ToListAsync();
        return View(minhasAvaliacoes);
    }


    // GET: Habitacao/EditarAvaliacoes/5
    /*public async Task<IActionResult> EditarAvaliacoes(int? id)
    {
        if (id == null) return NotFound();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var avaliacao = await _context.Avaliacoes
            .Include(a => a.Habitacao)
            .ThenInclude(h => h.DetalhesHabitacao)
            .FirstOrDefaultAsync(a => a.Id == id && a.Cliente.Id == userId);

        if (avaliacao == null) return NotFound();

        return View(new AvaliacaoEditar { Id = avaliacao.Id, Nota = avaliacao.Nota, Comentario = avaliacao.Comentario }); // Passe a avaliacao para a view
    }*/

    private bool AvaliacaoExists(int id)
    {
        return _context.Avaliacoes.Any(e => e.Id == id);
    }

    // POST: Habitacao/EditarAvaliacao/5
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarAvaliacao(int id, AvaliacaoEditar avaliacao)
    {
        if (id != avaliacao.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {

                // Busque a avaliação existente no banco de dados
                var avaliacaoExistente = await _context.Avaliacoes
                    .Include(a => a.Habitacao) // Inclua a propriedade Habitacao
                    .FirstOrDefaultAsync(a => a.Id == avaliacao.Id);


                if (avaliacaoExistente == null)
                {
                    return NotFound();
                }
                avaliacaoExistente.Nota = avaliacao.Nota;
                avaliacaoExistente.Comentario = avaliacao.Comentario;

                _context.Update(avaliacaoExistente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoExists(avaliacao.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(MinhasAvaliacoes));
        }
        return View(avaliacao);

      }
    }*/
}