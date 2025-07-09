using System;
using System.Threading;
using System.Threading.Tasks;

namespace OtChaim.Domain.EmergencyEvents;

public interface IEmergencyRepository
{
    Task<Emergency?> GetByIdAsync(Guid emergencyId, CancellationToken cancellationToken = default);
    Task AddAsync(Emergency emergency, CancellationToken cancellationToken = default);
    Task SaveAsync(Emergency emergency, CancellationToken cancellationToken = default);
} 