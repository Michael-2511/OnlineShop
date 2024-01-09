using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele produsului nu poate avea mai mult de 100 de caractere")]
        [MinLength(5, ErrorMessage = "Numele produsului trebuie sa aiba mai mult de 5 caractere")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
        public string Description { get; set; }

        public float? Rating { get; set; }

        [Required(ErrorMessage = "Produsul trebuie sa aiba un pret")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Produsul trebuie sa aiba o imagine")]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int? CategoryId { get; set; }
        public int? RequestId { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Category? Category { get; set; }
        // public virtual Request? Request { get; set; }

        // public virtual
        public string? UserId { get; set; }

        // un produs poate avea o colectie de review-uri
        public virtual ICollection<Review>? Reviews { get; set; }

        // lista de categorii
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }

    }
}