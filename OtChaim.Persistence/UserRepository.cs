using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence;

/// <summary>
/// Entity Framework Core implementation of the user repository.
/// </summary>
public class UserRepository(OtChaimDbContext context) : IUserRepository
{
    private readonly OtChaimDbContext _context = context;

    /// <inheritdoc/>
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.FindAsync([id], cancellationToken) ?? User.None;

    /// <inheritdoc/>
    public async Task<bool> RequiresSubscriptionApprovalAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.RequiresSubscriptionApproval())
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Users.ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await _context.Users.FindAsync([id], cancellationToken) ?? User.None;
        if (!user.Equals(User.None))
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
