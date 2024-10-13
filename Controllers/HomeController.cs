using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Flashcard.Models;
using Flashcard.Data;
using Microsoft.EntityFrameworkCore;

namespace Flashcard.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var baralhos = await _context.Baralhos.Select(b => new BaralhoModel{
            Id = b.Id,
            Nome = b.Nome,
            ContadorFlashcards = _context.Flashcards.Count(f => f.BaralhoId == b.Id)
        }).ToListAsync();
        return View(baralhos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
