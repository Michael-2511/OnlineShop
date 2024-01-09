namespace OnlineShop.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        public virtual ApplicationUser? User { get; set; }
        // un comentariu este postat de catre un user
        public string? UserId { get; set; }
    }
}
