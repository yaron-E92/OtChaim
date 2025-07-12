namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Repository interface for emergency entities.
/// </summary>
public interface IEmergencyRepository
{
    /// <summary>
    /// Gets an emergency by ID.
    /// </summary>
    Task<Emergency?> GetByIdAsync(Guid emergencyId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Adds a new emergency.
    /// </summary>
    Task AddAsync(Emergency emergency, CancellationToken cancellationToken = default);
    /// <summary>
    /// Saves an emergency entity.
    /// </summary>
    Task SaveAsync(Emergency emergency, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets all emergencies.
    /// </summary>
    Task<IReadOnlyList<Emergency>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets emergencies by status.
    /// </summary>
    Task<IReadOnlyList<Emergency>> GetByStatusAsync(EmergencyStatus status, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets all active emergencies.
    /// </summary>
    Task<IReadOnlyList<Emergency>> GetActiveAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets emergencies by user ID.
    /// </summary>
    Task<IReadOnlyList<Emergency>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
