using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Flashcard.Models
{
    public class FlashcardModel
    {
        public int Id { get;set; }

        public DateTime ultimaRevisao { get;set;}

        [Required(ErrorMessage = "A pergunta é obrigatória.")]
        public string? Pergunta { get;set; }

        [Required(ErrorMessage = "A pergunta é obrigatória.")]
        public string? Resposta { get;set; }

        [Required(ErrorMessage = "A pergunta é obrigatória.")]
        public int BaralhoId { get;set; }
        [ForeignKey("BaralhoId")]
        public BaralhoModel? Baralho { get;set; }

        [Required(ErrorMessage = "A pergunta é obrigatória.")]
        public int CategoriaId { get;set; }
        [ForeignKey("CategoriaId")]
        public CategoriaModel? Categoria { get;set; }
        public int RevisaoId {get;set;}
        [ForeignKey("RevisaoId")]
        public RevisaoModel Revisao {get;set;}
    }
}