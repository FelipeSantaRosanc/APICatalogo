﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APICatalogo.Models;


namespace APICatalogo.Model
{

    [Table("Categorias")]
    public class Categoria
    {

        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }
        [Key]
        public int CategoriaId { get; set; }
        [Required]  //Data anotation 
        [StringLength(80)]
        public string? Nome{ get; set; }
        [Required]
        [StringLength(300)] //Data anotation 
        public string? ImagemUrl{ get; set; }

        public ICollection<Produto>? Produtos { get; set; }

    }
}
