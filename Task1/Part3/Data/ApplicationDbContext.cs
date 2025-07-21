namespace Part3.Data;

using Microsoft.EntityFrameworkCore;
using Part3.Data.Models;

/// <summary>
/// Контекст БД.
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<MenuItem> MenuItems { get; set; }

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}
