using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Persistence;

public class EmergencyRepository(OtChaimDbContext context) : IEmergencyRepository
{
    private readonly OtChaimDbContext _context = context;

    public async Task<Emergency?> GetByIdAsync(Guid emergencyId, CancellationToken cancellationToken = default)
        => await _context.Emergencies.FindAsync([emergencyId], cancellationToken);

    public async Task AddAsync(Emergency emergency, CancellationToken cancellationToken = default)
    {
        await _context.Emergencies.AddAsync(emergency, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(Emergency emergency, CancellationToken cancellationToken = default)
    {
        _context.Emergencies.Update(emergency);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Emergency>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Emergencies.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Emergency>> GetByStatusAsync(EmergencyStatus status, CancellationToken cancellationToken = default)
        => await _context.Emergencies.Where(e => e.Status == status).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Emergency>> GetActiveAsync(CancellationToken cancellationToken = default)
        => await _context.Emergencies.Where(e => e.Status == EmergencyStatus.Active).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Emergency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Emergencies.Where(e => e.Responses.Any(r => r.UserId == userId)).ToListAsync(cancellationToken);
} 
