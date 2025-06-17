using System;
using System.Threading;
using System.Threading.Tasks;

namespace OtChaim.Domain.Users;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> RequiresSubscriptionApprovalAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SaveAsync(User user, CancellationToken cancellationToken = default);
} 