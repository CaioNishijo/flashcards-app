using Microsoft.EntityFrameworkCore;
using Flashcard.Models;

namespace Flashcard.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){  }

        public DbSet<BaralhoModel> Baralhos { get;set; }
        public DbSet<CategoriaModel> Categorias { get;set; }
        public DbSet<FlashcardModel> Flashcards { get;set; }
    }
}