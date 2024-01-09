using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System.Security.Cryptography.X509Certificates;

namespace OnlineShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // definirea relatiei many-to-many dintre Article si Bookmark

        //    base.OnModelCreating(modelBuilder);

        //    // definire primary key compus
        //    modelBuilder.Entity<ArticleBookmark>()
        //        .HasKey(ab => new { ab.Id, ab.ArticleId, ab.BookmarkId });


        //    // definire relatii cu modelele Bookmark si Article (FK)

        //    modelBuilder.Entity<ArticleBookmark>()
        //        .HasOne(ab => ab.Article)
        //        .WithMany (ab => ab.ArticleBookmarks)
        //        .HasForeignKey(ab => ab.ArticleId);

        //    modelBuilder.Entity<ArticleBookmark>()
        //        .HasOne(ab => ab.Bookmark)
        //        .WithMany(ab => ab.ArticleBookmarks)
        //        .HasForeignKey(ab => ab.BookmarkId);
        //}
    }
}
