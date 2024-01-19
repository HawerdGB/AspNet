using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppStore.Models.Domain
{
    public class Categoria
    {
        [Key]
        [Required]
        public int Id {get; set;}
        [Required]
        public string? Nombre {get; set;}
        public virtual ICollection<Libro>? LibroRelationList {get; set;}
        public virtual ICollection<LibroCategoria>? LibroCategoriaRelationList {get; set;}
    }
}