using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public virtual ICollection<Review>? Reviews { get; set; }

        // un user poate posta mai multe articole
        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<Request>? Requests { get; set; }

        // un user poate posta mai multe articole
        public virtual ICollection<Cart>? Carts{ get; set; }
    }
}
