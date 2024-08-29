
using ECommerce.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.DataAccess.Data;

public class ApplicationDbContext : DbContext
{

     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Order> Orders => Set<Order>();
     public DbSet<Payment> Payments => Set<Payment>();
     public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
     public DbSet<Product> Products => Set<Product>();
     public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
     public DbSet<Review> Reviews => Set<Review>();
    public DbSet<User> Users => Set<User>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}