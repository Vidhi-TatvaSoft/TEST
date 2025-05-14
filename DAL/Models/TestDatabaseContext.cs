using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public class TestDatabaseContext : DbContext
{
    public DbSet<Users> Users { get; set; }

    public DbSet<Roles> Roles { get; set; }

    public TestDatabaseContext(DbContextOptions<TestDatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TestDatabase;Username=postgres; password=Tatva@123");
    
}
