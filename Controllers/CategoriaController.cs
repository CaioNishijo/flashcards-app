using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Flashcard.Models;
using Flashcard.Data;
using Microsoft.EntityFrameworkCore;


namespace Flashcard.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly DataContext _context;

        public CategoriaController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Nome")] CategoriaModel categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}