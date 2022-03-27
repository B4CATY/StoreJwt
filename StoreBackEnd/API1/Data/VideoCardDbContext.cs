using API1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace API1.Data
{
    public class VideoCardDbContext : DbContext
    {
        public VideoCardDbContext(DbContextOptions<VideoCardDbContext> options) : base(options)
        {
            CreateIfEmpty();
        }
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<VideoCart> Videocarts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");
                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.HasIndex(e => e.Email, "user_login_key")
                   .IsUnique();
                entity.Property(e => e.Email)
                   .HasMaxLength(30)
                   .HasColumnName("login");
            });



            modelBuilder.Entity<VideoCart>(entity =>
            {
                entity.ToTable("videocart");

                entity.HasIndex(e => e.NameProduct, "product_videocart_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.NameProduct)
                    .HasMaxLength(30)
                    .HasColumnName("name_product");

                entity.Property(e => e.Price).HasColumnName("price");

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");
            });


            modelBuilder.Entity<Cart>()
                .HasMany(p => p.VideoCarts)
                .WithMany(p => p.Carts)
                .UsingEntity<Dictionary<string, object>>(
                    "CartVideoCart",
                    j => j
                        .HasOne<VideoCart>()
                        .WithMany()
                        .HasForeignKey("VideoCartId")
                        .HasConstraintName("FK_CartVideoCart_VideoCarts_VideoCartId")
                        .OnDelete(DeleteBehavior.ClientCascade),
                    j => j
                        .HasOne<Cart>()
                        .WithMany()
                        .HasForeignKey("CartId")
                        .HasConstraintName("FK_CartVideoCart_Carts_CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        );
        }


        private void CreateIfEmpty()
        {
            if (Database.EnsureCreated())
            {
                var cat = new Category { Name = "GIGABYTE" };
                var cat1 = new Category { Name = "ASUS" };


                var videog = new VideoCart
                {
                    Category = cat,
                    Description = " ",
                    NameProduct = "GIGABYTE GeForce RTX 3060",
                    Price = 25866
                };
                var video1g = new VideoCart
                {
                    Category = cat,
                    Description = " ",
                    NameProduct = "GIGABYTE GeForce RTX 3070",
                    Price = 37700
                };
                var video2g = new VideoCart
                {
                    Category = cat,
                    Description = " ",
                    NameProduct = "GIGABYTE GeForce RTX 3080",
                    Price = 42700
                };
                var video3g = new VideoCart
                {
                    Category = cat,
                    Description = " ",
                    NameProduct = "GIGABYTE GeForce RTX 3090",
                    Price = 50720
                };

                var videoa = new VideoCart
                {
                    Category = cat1,
                    Description = " ",
                    NameProduct = "ASUS GeForce RTX 3060",
                    Price = 27866
                };
                var video1a = new VideoCart
                {
                    Category = cat1,
                    Description = " ",
                    NameProduct = "ASUS GeForce RTX 3070",
                    Price = 36660
                };
                var video2a = new VideoCart
                {
                    Category = cat1,
                    Description = " ",
                    NameProduct = "ASUS GeForce RTX 3080",
                    Price = 42700
                };
                var video3a = new VideoCart
                {
                    Category = cat1,
                    Description = " ",
                    NameProduct = "ASUS GeForce RTX 3090",
                    Price = 50700
                };



                var cat2 = new Category { Name = "Palit" };


                var videop = new VideoCart
                {
                    Category = cat2,
                    Description = " ",
                    NameProduct = "Palit GeForce RTX 3060",
                    Price = 24866
                };
                var video1p = new VideoCart
                {
                    Category = cat2,
                    Description = " ",
                    NameProduct = "Palit GeForce RTX 3070",
                    Price = 37660
                };
                var video2p = new VideoCart
                {
                    Category = cat2,
                    Description = " ",
                    NameProduct = "Palit GeForce RTX 3080",
                    Price = 43700
                };
                var video3p = new VideoCart
                {
                    Category = cat2,
                    Description = " ",
                    NameProduct = "Palit GeForce RTX 3090",
                    Price = 51700
                };

                var cat3 = new Category { Name = "MSI" };

                var videom = new VideoCart
                {
                    Category = cat3,
                    Description = " ",
                    NameProduct = "MSI GeForce RTX 3060",
                    Price = 22866
                };
                var video1m = new VideoCart
                {
                    Category = cat3,
                    Description = " ",
                    NameProduct = "MSI GeForce RTX 3070",
                    Price = 38660
                };
                var video2m = new VideoCart
                {
                    Category = cat3,
                    Description = " ",
                    NameProduct = "MSI GeForce RTX 3080",
                    Price = 45700
                };
                var video3m = new VideoCart
                {
                    Category = cat3,
                    Description = " ",
                    NameProduct = "MSI GeForce RTX 3090",
                    Price = 55700
                };

                Videocarts.AddRange(new List<VideoCart> { videog, video1g, video2g, video3g });
                Videocarts.AddRange(new List<VideoCart> { videoa, video1a, video2a, video3a });
                Videocarts.AddRange(new List<VideoCart> { videom, video1m, video2m, video3m });
                Videocarts.AddRange(new List<VideoCart> { videop, video1p, video2p, video3p });

                Users.Add(
                    new User
                    {
                        Email = "admin@gmail.com",
                    });
                //Carts.Add();

                SaveChanges();
            }
        }
    }
    
}
