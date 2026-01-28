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
        public DbSet<FaqEntry> FaqEntries { get; set; }



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
                .WithMany(o => o.Vouchers)
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

            builder.Entity<FaqEntry>().HasData(
        new FaqEntry
        {
            Id = 1,
            Question = "Каква е валидността?",
            Answer = "Всички наши ваучери са с валидност, избрана от Вас при покупката (3, 6 или 12 месеца). Можете да видите срока на самия ваучер.",
            Keywords = "валидност, срок, време, изтича, дата"
        },
        new FaqEntry
        {
            Id = 2,
            Question = "Как става доставката?",
            Answer = "Ваучерът се получава дигитално на имейла Ви веднага след успешна поръчка. Можете да го разпечатате или покажете от телефона.",
            Keywords = "доставка, куриер, еконт, спиди, пристига, получа"
        },
       
        new FaqEntry
        {
            Id = 3,
            Question = "Мога ли да върна ваучер?",
            Answer = "Да, съгласно закона имате право на отказ в рамките на 14 дни, ако ваучерът все още не е използван/резервиран.",
            Keywords = "върна, връщане, отказ, рекламация, пари"
        },
        new FaqEntry
        {
            Id = 4,
            Question = "Как да резервирам?",
            Answer = "След като купите ваучер, влезте в списъка с поръчките си и използвайте бутона 'Резервация' срещу съответната поръчка.",
            Keywords = "резерв, запаз, час, дата, използ"
        },
        new FaqEntry
        {
            Id = 5,
            Question = "Нужна ли е регистрация?",
            Answer = "За да направите поръчка и да имате достъп до историята на Вашите ваучери и резервации, е необходимо да си създадете профил в GiftRide.",
            Keywords = "регистрация, профил, акаунт, вход, login, register"
        },
        new FaqEntry
        {
            Id = 6,
            Question = "Къде са ми ваучерите?",
            Answer = "След успешна покупка, всички Ваши ваучери се съхраняват в секция 'Моите поръчки' във Вашия профил, откъдето можете да ги разгледате по всяко време.",
            Keywords = "къде, намират, ваучерите, поръчките, orders, история, изтегля"
        }
    );
        }
    }
}
