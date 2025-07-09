using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence;

public class OtChaimDbContext(DbContextOptions<OtChaimDbContext> options) : DbContext(options)
{
    public DbSet<Emergency> Emergencies { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: Configure Emergency, User, and related entities as needed
        base.OnModelCreating(modelBuilder);
    }
}
