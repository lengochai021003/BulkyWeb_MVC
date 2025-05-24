using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName="Action", DisplayOrder=1},
                new Category { CategoryId = 2, CategoryName = "SciFi", DisplayOrder = 2 },
                new Category { CategoryId = 3, CategoryName = "History", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Company>().HasData(
              new Company { Id = 1, Name = "Tech1", StreetAddress="123 tech st", City="tech city", PostalCode="123123", State="tl", PhoneNumber="545644865"},
              new Company { Id = 2, Name = "Tech2", StreetAddress="123 tech st", City="tech city", PostalCode="123123", State="tl", PhoneNumber="545644865"},
              new Company { Id = 3, Name = "Tech3", StreetAddress="123 tech st", City="tech city", PostalCode="123123", State="tl", PhoneNumber= "545644865" }
              );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    productID = 1,
                    titleProduct = "Fortune of Time",
                    Author = "Billy Spark",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    listPrice = 99,
                    priceProduct = 90,
                    priceProductFor50 = 85,
                    priceProductFor100 = 80,
                    CategoryId = 1,
                    ImageUrl=""
                },
                new Product
                {
                    productID = 2,
                    titleProduct = "Dark Skies",
                    Author = "Nancy Hoover",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    listPrice = 40,
                    priceProduct = 30,
                    priceProductFor50 = 25,
                    priceProductFor100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""

                },
                new Product
                {
                    productID = 3,
                    titleProduct = "Vanish in the Sunset",
                    Author = "Julian Button",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    listPrice = 55,
                    priceProduct = 50,
                    priceProductFor50 = 40,
                    priceProductFor100 = 35,
                    CategoryId = 2,
                    ImageUrl = ""

                },
                new Product
                {
                    productID = 4,
                    titleProduct = "Cotton Candy",
                    Author = "Abby Muscles",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    listPrice = 70,
                    priceProduct = 65,
                    priceProductFor50 = 60,
                    priceProductFor100 = 55,
                    CategoryId = 2,
                    ImageUrl = ""

                },
                new Product
                {
                    productID = 5,
                    titleProduct = "Rock in the Ocean",
                    Author = "Ron Parker",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    listPrice = 30,
                    priceProduct = 27,
                    priceProductFor50 = 25,
                    priceProductFor100 = 20,
                    CategoryId = 3,
                    ImageUrl = ""

                },
                new Product
                {
                    productID = 6,
                    titleProduct = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    productDecsciption = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    listPrice = 25,
                    priceProduct = 23,
                    priceProductFor50 = 22,
                    priceProductFor100 = 20,
                    CategoryId = 3,
                    ImageUrl = ""
                }

                );
        }
    }
}
