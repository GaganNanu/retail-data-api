using Microsoft.EntityFrameworkCore;

namespace retail_data_api.Models;

public class RetailContext : DbContext
{
    public RetailContext(DbContextOptions<RetailContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Joined> Joined { get; set; } = null!;
    public DbSet<Product> Product { get; set; } = null!;
    public DbSet<Household> Household { get; set; } = null!;
    public DbSet<Transaction> Transaction { get; set; } = null!;
}