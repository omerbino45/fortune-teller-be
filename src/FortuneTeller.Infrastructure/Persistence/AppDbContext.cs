using FortuneTeller.Domain.Entities;
using FortuneTeller.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FortuneTeller.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Worry> Worries => Set<Worry>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorryConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
