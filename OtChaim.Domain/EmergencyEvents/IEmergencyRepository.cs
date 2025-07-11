namespace OtChaim.Domain.EmergencyEvents;

public interface IEmergencyRepository
{
    Task<Emergency?> GetByIdAsync(Guid emergencyId, CancellationToken cancellationToken = default);
    Task AddAsync(Emergency emergency, CancellationToken cancellationToken = default);
    Task SaveAsync(Emergency emergency, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Emergency>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Emergency>> GetByStatusAsync(EmergencyStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Emergency>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Emergency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
} 
