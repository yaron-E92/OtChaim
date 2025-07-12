using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OtChaim.Persistence;

/// <summary>
/// Factory for creating OtChaim database context at design time.
/// </summary>
public class DesignTimeOtChaimDbContextFactory : IDesignTimeDbContextFactory<OtChaimDbContext>
{
    /// <summary>
    /// Creates a new instance of the OtChaim database context.
    /// </summary>
    public OtChaimDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OtChaimDbContext>();
        // Default to InMemory for design-time, can be changed as needed
        optionsBuilder.UseInMemoryDatabase("OtChaimDb");
        return new OtChaimDbContext(optionsBuilder.Options);
    }
}