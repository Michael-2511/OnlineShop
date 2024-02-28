using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<Request>? Requests { get; set; }

        public virtual Cart? Cart { get; set; }

        // atribute suplimentare adaugate pentru user
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // variabila in care vom retine rolurile existente in baza de date
        // pentru popularea unui dropdown list
        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
