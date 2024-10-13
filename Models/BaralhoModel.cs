using System.ComponentModel.DataAnnotations;

namespace Flashcard.Models
{
    public class BaralhoModel
    {
        public int Id { get;set; }
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get;set; }
        public List<FlashcardModel>? Flashcards { get;set; }
        public int ContadorFlashcards { get;set; }
    }
}