using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence.Tests;

[TestFixture]
public class UserRepositoryTests
{
    private OtChaimDbContext _context = null!;
    private UserRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        DbContextOptions<OtChaimDbContext> options = new DbContextOptionsBuilder<OtChaimDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new OtChaimDbContext(options);
        _repository = new UserRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task SaveAndGetById_Works()
    {
        // Arrange
        User user = new User("test", "test@example.com", "00000000");

        // Act
        await _repository.AddAsync(user);
        User loaded = await _repository.GetByIdAsync(user.Id);

        // Assert
        loaded.Should().NotBeNull();
        loaded.Id.Should().Be(user.Id);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        User user1 = new User("a", "a@example.com", "1111111");
        User user2 = new User("b", "b@example.com", "2222222");
        await _repository.AddAsync(user1);
        await _repository.AddAsync(user2);

        // Act
        IReadOnlyList<User> all = await _repository.GetAllAsync();

        // Assert
        all.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByEmailAsync_ReturnsCorrectUser()
    {
        // Arrange
        const string Email = "findme@example.com";
        User user = new User("findme", Email, "0000000");
        await _repository.AddAsync(user);

        // Act
        User? found = await _repository.GetByEmailAsync(Email);

        // Assert
        found.Should().NotBeNull();
        found!.Email.Should().Be(Email);
    }

    [Test]
    public async Task DeleteAsync_RemovesUser()
    {
        // Arrange
        User user = new User("delete", "delete@example.com", "0000000");
        await _repository.AddAsync(user);

        // Act
        await _repository.DeleteAsync(user.Id);
        User loaded = await _repository.GetByIdAsync(user.Id);

        // Assert
        loaded.Should().Be(User.None);
    }

    [Test]
    public async Task RequiresSubscriptionApprovalAsync_ReturnsTrue_WhenApprovalIsRequired()
    {
        // Arrange
        User user = new User("approval", "approval@example.com", "0000000");
        await _repository.AddAsync(user);

        // Act
        bool requires = await _repository.RequiresSubscriptionApprovalAsync(user.Id);

        // Assert
        requires.Should().BeTrue();
    }

    [Test]
    public async Task RequiresSubscriptionApprovalAsync_ReturnsFalse_WhenApprovalIsNotRequired()
    {
        // Arrange
        User user = new User("noapproval", "noapproval@example.com", "0000000");
        user.ToggleApproval();
        await _repository.AddAsync(user);

        // Act
        bool requires = await _repository.RequiresSubscriptionApprovalAsync(user.Id);

        // Assert
        requires.Should().BeFalse();
    }
}
