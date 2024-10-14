namespace Flashcard.Models
{
    public class RevisaoModel
    {
        public int Id { get;set; }
        public DateTime proximaRevisao { get;set; }
        public int revisoesRealizadas { get;set; }
        public DateTime DataCriacao { get;set; }

        public int FlashcardId { get;set; }
        public FlashcardModel Flashcard { get;set; }
    }
}