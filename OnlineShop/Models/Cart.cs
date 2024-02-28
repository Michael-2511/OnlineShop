namespace OnlineShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public float? Price { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<CartProduct>? CartProducts { get; set; }
    }
}
