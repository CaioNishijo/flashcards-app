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
            var flashcards = await _context.Flashcards
            .Include(f => f.Categoria)
            .Include(f => f.Revisao)
            .Where(flashcard => flashcard.BaralhoId == id)
            .ToListAsync();

            var hoje = DateTime.UtcNow;

            var view = new FlashcardsFiltradoDataRevisao
            {
                Hoje = flashcards.Where(f => f.Revisao.proximaRevisao <= hoje).ToList(),
                UmDia = flashcards.Where(f => f.Revisao.proximaRevisao > hoje && f.Revisao.proximaRevisao <= hoje.AddDays(1)).ToList(),
                TresDias = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(1) && f.Revisao.proximaRevisao < hoje.AddDays(3)).ToList(),
                SeteDias = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(3) && f.Revisao.proximaRevisao < hoje.AddDays(7)).ToList(),
                TrintaDias = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(7) && f.Revisao.proximaRevisao < hoje.AddDays(30)).ToList()
            };

            return View(view);
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
                    var revisao = new RevisaoModel();
                    var data_atual = DateTime.Now;
                    revisao.DataCriacao = data_atual.ToUniversalTime();
                    revisao.proximaRevisao = data_atual.AddHours(24).ToUniversalTime();
                    revisao.revisoesRealizadas = 0;
                    
                    _context.Revisoes.Add(revisao);
                    await _context.SaveChangesAsync();

                    flashcard.RevisaoId = revisao.Id;

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

        public async Task<IActionResult> Study(int baralhoid, int? currentId, int intervalo, bool? acertou)
        {
            // Pega todos os cards
            var flashcards = await _context.Flashcards.Include(f => f.Revisao).Include(f => f.Categoria)
            .Where(flashcard => flashcard.BaralhoId == baralhoid)
            .ToListAsync();

            // Filtra os cards
            var hoje = DateTime.UtcNow;

            if(intervalo == 0)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao <= hoje).ToList();
            }
            else if(intervalo == 1)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje && f.Revisao.proximaRevisao < hoje.AddDays(1)).ToList();
            }
            else if(intervalo == 2)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(1) && f.Revisao.proximaRevisao < hoje.AddDays(3)).ToList();
            }
            else if(intervalo == 3)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(3) && f.Revisao.proximaRevisao < hoje.AddDays(7)).ToList();
            }
            else if(intervalo == 4)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(7) && f.Revisao.proximaRevisao < hoje.AddDays(30)).ToList();
            }

            if (flashcards.Count == 0)
            {
                return NotFound("Nenhum flashcard encontrado para este baralho.");
            }

            // Pega o card atual
            FlashcardModel flashcardAtual;
            
            if (currentId.HasValue)
            {
                flashcardAtual = flashcards.FirstOrDefault(f => f.Id == currentId);
            }
            else
            {
                flashcardAtual = flashcards.FirstOrDefault();
            }

            // Cálculo dos índices
            int currentIndex = flashcards.IndexOf(flashcardAtual);
            int nextIndex = (currentIndex + 1) % flashcards.Count;

            if(acertou.HasValue && acertou == true)
            {
                flashcards[currentIndex - 1].Revisao.UltimaRevisao = hoje;
                if( flashcards[currentIndex - 1].Revisao.revisoesRealizadas == 0)
                {
                    flashcards[currentIndex - 1].Revisao.proximaRevisao = hoje.AddDays(3);
                }
                else if(flashcards[currentIndex - 1].Revisao.revisoesRealizadas == 1)
                {
                    flashcards[currentIndex - 1].Revisao.proximaRevisao = hoje.AddDays(7);
                }
                else if(flashcards[currentIndex - 1].Revisao.revisoesRealizadas == 2)
                {
                    flashcards[currentIndex - 1].Revisao.proximaRevisao = hoje.AddDays(30);
                }
                else
                {
                    flashcards[currentIndex - 1].Revisao.proximaRevisao = hoje.AddDays(7);
                }
                flashcards[currentIndex -1].Revisao.revisoesRealizadas++;   

                _context.Flashcards.Update(flashcards[currentIndex - 1]);
            }
            else if(acertou.HasValue && acertou == false)
            {
                flashcards[currentIndex - 1].Revisao.UltimaRevisao = hoje;
                flashcards[currentIndex - 1].Revisao.proximaRevisao = hoje.AddDays(1);
                _context.Flashcards.Update(flashcards[currentIndex - 1]);
            }

            bool ultimo = false;
            if(currentIndex == flashcards.Count - 1)
            {
                ViewBag.isLastFlashcard = true;
            }
            else
            {
                ViewBag.isLastFlashcard = false;
            }

            // Passagem de parâmetros e view
            ViewBag.NextFlashcardId = flashcards[nextIndex].Id;
            ViewBag.BaralhoId = baralhoid; 
            ViewBag.Intervalo = intervalo;

            await _context.SaveChangesAsync();
            return View(flashcardAtual);
        }
        public async Task<IActionResult> StudyResults(int baralhoid, int? currentId, int intervalo, bool? acertou)
        {

            // Filtra os cards
           var flashcards = await _context.Flashcards.Include(f => f.Revisao).Include(f => f.Categoria)
            .Where(flashcard => flashcard.BaralhoId == baralhoid)
            .ToListAsync();

            // Filtra os cards
            var hoje = DateTime.UtcNow;

            if(intervalo == 0)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao <= hoje).ToList();
            }
            else if(intervalo == 1)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje && f.Revisao.proximaRevisao < hoje.AddDays(1)).ToList();
            }
            else if(intervalo == 2)
            {
                flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(1) && f.Revisao.proximaRevisao < hoje.AddDays(3)).ToList();
            }
            else if(intervalo == 3)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(3) && f.Revisao.proximaRevisao < hoje.AddDays(7)).ToList();
            }
            else if(intervalo == 4)
            {
                flashcards = flashcards.Where(f => f.Revisao.proximaRevisao > hoje.AddDays(7) && f.Revisao.proximaRevisao < hoje.AddDays(30)).ToList();
            }

            if (flashcards.Count == 0)
            {
                return NotFound("Nenhum flashcard encontrado para este baralho.");
            }

            // Pega o card atual
            FlashcardModel flashcardAtual;
            
            if (currentId.HasValue)
            {
                flashcardAtual = flashcards.FirstOrDefault(f => f.Id == currentId);
            }
            else
            {
                flashcardAtual = flashcards.FirstOrDefault();
            }

            // Cálculo dos índices
            int currentIndex = flashcards.IndexOf(flashcardAtual);

            if(acertou.HasValue && acertou == true)
            {
                flashcards[currentIndex].Revisao.UltimaRevisao = hoje;
                if( flashcards[currentIndex].Revisao.revisoesRealizadas == 0)
                {
                    flashcards[currentIndex].Revisao.proximaRevisao = hoje.AddDays(3);
                }
                else if(flashcards[currentIndex].Revisao.revisoesRealizadas == 1)
                {
                    flashcards[currentIndex].Revisao.proximaRevisao = hoje.AddDays(7);
                }
                else if(flashcards[currentIndex].Revisao.revisoesRealizadas == 2)
                {
                    flashcards[currentIndex].Revisao.proximaRevisao = hoje.AddDays(30);
                }
                else
                {
                    flashcards[currentIndex].Revisao.proximaRevisao = hoje.AddDays(7);
                }
                flashcards[currentIndex].Revisao.revisoesRealizadas++;   

                _context.Flashcards.Update(flashcards[currentIndex]);
            }
            else if(acertou.HasValue && acertou == false)
            {
                flashcards[currentIndex].Revisao.UltimaRevisao = hoje;
                flashcards[currentIndex].Revisao.proximaRevisao = hoje.AddDays(1);
                _context.Flashcards.Update(flashcards[currentIndex]);
            }
            
            await _context.SaveChangesAsync();

            return View();
        }
    }
}