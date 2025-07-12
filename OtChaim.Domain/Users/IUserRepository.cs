namespace OtChaim.Domain.Users;

/// <summary>
/// Repository interface for user entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Determines if a user requires subscription approval.
    /// </summary>
    Task<bool> RequiresSubscriptionApprovalAsync(Guid userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Saves a user entity.
    /// </summary>
    Task SaveAsync(User user, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets all users.
    /// </summary>
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a user by email address.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Adds a new user.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
