using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence;

public class OtChaimDbContext(DbContextOptions<OtChaimDbContext> options) : DbContext(options)
{
    public DbSet<Emergency> Emergencies { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Emergency configuration
        modelBuilder.Entity<Emergency>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.OwnsOne(e => e.Location, ConfigureLocation);
            builder.OwnsMany(e => e.AffectedAreas, area =>
            {
                area.WithOwner();
                area.OwnsOne(a => a.Center, ConfigureLocation);
                area.Property(a => a.RadiusInMeters);
            });
            builder.OwnsMany(e => e.Responses, resp =>
            {
                resp.Property(r => r.UserId);
                resp.Property(r => r.IsSafe);
                resp.Property(r => r.Message);
                resp.Property(r => r.RespondedAt);
            });
        });

        // User configuration
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.OwnsMany(u => u.NotificationChannels, nc =>
            {
                nc.Property(n => n.ChannelType);
                nc.Property(n => n.Address);
            });
            builder.OwnsMany(u => u.Subscriptions, sub =>
            {
                sub.Property(s => s.SubscriberId);
                sub.Property(s => s.SubscribedToId);
                sub.Property(s => s.Status);
                sub.Property(s => s.CreatedAt);
                sub.Property(s => s.ApprovedAt);
            });
        });

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureLocation<T>(OwnedNavigationBuilder<T, Location> loc) where T : class
    {
        loc.WithOwner();
        loc.Property(l => l.Latitude);
        loc.Property(l => l.Longitude);
        loc.Property(l => l.Description);
    }
}
