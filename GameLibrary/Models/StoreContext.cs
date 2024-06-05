
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Models;

namespace GameLibrary.Models
{
    public class StoreContext : IdentityDbContext<ApplicationUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<BookCart> BookCarts { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<GameLibrary.Models.SCartItem> SCartItems { get; set; }
        public DbSet<Recommend> Recommends { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookCart>()
                .HasKey(c => new { c.ISBN, c.CartID });
            modelBuilder.Entity<BookOrder>()
                .HasKey(c => new { c.ISBN, c.OrderID });

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<GameLibrary.Models.SCart> SCart { get; set; }



        public DbSet<JobSection> JobSections { get; set; }



        public DbSet<GameLibrary.Models.JobApplication> JobApplication { get; set; }
        public DbSet<Eval> Evals { get; set; }

    }
}
