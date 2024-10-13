using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Flashcard.Models;
using Flashcard.Data;
using Microsoft.EntityFrameworkCore;
using Flashcard.Controllers;

namespace Flashcard.Models
{
    public class BaralhoController : Controller
    {
        private readonly DataContext _context;

        public BaralhoController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Nome")] BaralhoModel baralho)
        {
            if(ModelState.IsValid)
            {
                _context.Baralhos.Add(baralho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(baralho);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var baralho = await _context.Baralhos.FindAsync(id);
            return View(baralho);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Nome")] BaralhoModel baralho)
        {
            if(ModelState.IsValid)
            {
                _context.Baralhos.Update(baralho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(baralho);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var baralho = await _context.Baralhos.FindAsync(id);
            return View(baralho);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteBaralho(int id)
        {
            var baralho = await _context.Baralhos.FindAsync(id);
            if(baralho != null){
                 _context.Baralhos.Remove(baralho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
           return View(baralho);
        }
    }
}