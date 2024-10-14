using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Flashcard.Models;
using Flashcard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Flashcard.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly DataContext _context;

        public FlashcardController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id){
            var flashcards = await _context.Flashcards.Include(f => f.Categoria).Where(flashcard => flashcard.BaralhoId == id).ToListAsync();
            return View(flashcards);
        }
        public IActionResult Create(int id)
        {
            var categorias = _context.Categorias.ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");

            var flashcard = new FlashcardModel
            {
                BaralhoId = id
            };
            return View(flashcard);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Pergunta, Resposta, BaralhoId, CategoriaId")] FlashcardModel flashcard)
        {
                 if (ModelState.IsValid)
                {
                    flashcard.DataCriacao = DateTime.UtcNow;
                    _context.Flashcards.Add(flashcard);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                var categorias = _context.Categorias.ToList();
                ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", flashcard.CategoriaId);
                return View(flashcard);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            var categorias = _context.Categorias.ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", flashcard.CategoriaId);
            flashcard.BaralhoId = flashcard.BaralhoId;
            return View(flashcard);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Pergunta, Resposta, BaralhoId, DataCriacao, CategoriaId")] FlashcardModel flashcard)
        {
            if(ModelState.IsValid)
            {
                flashcard.BaralhoId = flashcard.BaralhoId;
                flashcard.DataCriacao = flashcard.DataCriacao.ToUniversalTime();
                _context.Flashcards.Update(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Flashcard", new { id = flashcard.BaralhoId });
            }
            var categorias = _context.Categorias.ToList();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", flashcard.CategoriaId);
            return View(flashcard);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            return View(flashcard);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteFlashcard(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            if(flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Flashcard", new { id = flashcard.BaralhoId});
            }
            return View(flashcard);
        }

        public async Task<IActionResult> Study(int baralhoid, int? currentId)
        {
            var flashcards = await _context.Flashcards.Include(f => f.Categoria)
            .Where(flashcard => flashcard.BaralhoId == baralhoid)
            .ToListAsync();

            if (flashcards.Count == 0)
            {
                return NotFound("Nenhum flashcard encontrado para este baralho.");
            }

            FlashcardModel flashcardAtual;
            
            if (currentId.HasValue)
            {
                flashcardAtual = flashcards.FirstOrDefault(f => f.Id == currentId);
            }
            else
            {
                flashcardAtual = flashcards.FirstOrDefault(); // Começa pelo primeiro flashcard se currentId não for fornecido
            }

            int currentIndex = flashcards.IndexOf(flashcardAtual);
            int nextIndex = (currentIndex + 1) % flashcards.Count;

            bool isLastFlashcard = currentIndex == flashcards.Count - 1;
            if(isLastFlashcard)
            {
                ViewBag.isLastFlashcard = isLastFlashcard;
            }
            else
            {
                ViewBag.isLastFlashcard = false;
            }

            ViewBag.NextFlashcardId = flashcards[nextIndex].Id;
            ViewBag.BaralhoId = baralhoid; 

            return View(flashcardAtual);
        }
        public IActionResult StudyResults()
        {
            return View();
        }
    }
}