using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence;

public class UserRepository(OtChaimDbContext context) : IUserRepository
{
    private readonly OtChaimDbContext _context = context;

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.FindAsync([id], cancellationToken) ?? User.None;

    public async Task<bool> RequiresSubscriptionApprovalAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.RequiresSubscriptionApproval())
            .FirstOrDefaultAsync(cancellationToken);

    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // Recommended additional methods:
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Users.ToListAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await _context.Users.FindAsync([id], cancellationToken) ?? User.None;
        if (user != User.None)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 
