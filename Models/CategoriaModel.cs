using System.ComponentModel.DataAnnotations;

namespace Flashcard.Models
{
    public class CategoriaModel
    {
        [Key]
        public int Id { get;set; }
        public string Nome { get;set; }

        public List<FlashcardModel>? Flashcards { get;set; }
    }
}