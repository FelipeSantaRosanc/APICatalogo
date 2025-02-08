using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }

        [Required]  //Data anotation 
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)] //Data anotation 
        public string? ImagemUrl { get; set; }

    }
}
