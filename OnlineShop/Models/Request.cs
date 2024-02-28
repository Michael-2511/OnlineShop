using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Descrierea cererii este obligatorie")]
        public string Description { get; set; }
        public string Status { get; set; }

        // PENTRU PRODUS ***
        [Required(ErrorMessage = "Numele produsului este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele produsului nu poate avea mai mult de 100 de caractere")]
        [MinLength(5, ErrorMessage = "Numele produsului trebuie sa aiba mai mult de 5 caractere")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Produsul trebuie sa aiba un pret")]
        public float ProductPrice { get; set; }
        [Required(ErrorMessage = "Produsul trebuie sa aiba o imagine")]
        public string ProductPicture { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int? CategoryId { get; set; }
        // *** PENTRU PRODUS

        // nush pentru ce e User, 0 references la momentul comentariului
        public virtual ApplicationUser? User { get; set; }
        // un comentariu este postat de catre un user
        public string? UserId { get; set; }
        public virtual Category? Category { get; set; }
        // pentru dropdown menu
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }

        // public virtual ICollection<Product>? Products { get; set; }
    }
}