using GiftRide.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GiftRide.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }       
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Validity> Validities { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

//            // Казваме на базата: Не трий ваучерите автоматично, ако се изтрие поръчка или продукт!
//            Error Number:1785,State: 0,Class: 16

             //Introducing FOREIGN KEY constraint 'FK_Vouchers_Products_ProductId' on table 'Vouchers' may cause cycles or multiple cascade paths.Specify
             //ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.

            //Could not create constraint or index.See previous errors.
            builder.Entity<Voucher>()
                .HasOne(v => v.Order)
                .WithMany()
                .HasForeignKey(v => v.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Voucher>()
                .HasOne(v => v.Product)
                .WithMany() 
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PromoCode>().HasData(
                new PromoCode
                {
                    Id = 1,
                    Code = "PROMO10",
                    DiscountPercent = 10,
                    IsActive = true
                },
                new PromoCode
                {
                    Id = 2,
                    Code = "PROMO20",
                    DiscountPercent = 20,
                    IsActive = true
                }
            );
        }
    }
}
