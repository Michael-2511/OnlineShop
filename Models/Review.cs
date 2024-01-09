using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Continutul recenziei este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        // un review apartine unui produs
        public int? ProductId { get; set; }
        // un review este postat de catre un user
        // public string? UserId {  get; set; }

        public virtual ApplicationUser? User { get; set; }

        // un comentariu este postat de catre un user
        public string? UserId { get; set; }
    }
}
