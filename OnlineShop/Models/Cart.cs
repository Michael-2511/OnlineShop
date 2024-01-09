namespace OnlineShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
