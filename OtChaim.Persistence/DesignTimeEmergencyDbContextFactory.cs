using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OtChaim.Persistence;

public class DesignTimeOtChaimDbContextFactory : IDesignTimeDbContextFactory<OtChaimDbContext>
{
    public OtChaimDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OtChaimDbContext>();
        // Default to InMemory for design-time, can be changed as needed
        optionsBuilder.UseInMemoryDatabase("OtChaimDb");
        return new OtChaimDbContext(optionsBuilder.Options);
    }
} 