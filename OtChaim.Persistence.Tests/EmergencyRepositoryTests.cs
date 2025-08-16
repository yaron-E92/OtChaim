using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Persistence.Tests;

[TestFixture]
public class EmergencyRepositoryTests
{
    private OtChaimDbContext _context = null!;
    private EmergencyRepository _repository = null!;

    private static Location TestLocation => new(0, 0);

    [SetUp]
    public void SetUp()
    {
        DbContextOptions<OtChaimDbContext> options = new DbContextOptionsBuilder<OtChaimDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new OtChaimDbContext(options);
        _repository = new EmergencyRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAndGetById_Works()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());

        // Act
        await _repository.AddAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded.Should().NotBeNull();
        loaded!.Id.Should().Be(emergency.Id);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllEmergencies()
    {
        // Arrange
        var e1 = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var e2 = new Emergency(Guid.Empty, Guid.Empty, new Location(1, 1));
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> all = await _repository.GetAllAsync();

        // Assert
        all.Should().HaveCount(2);
        all.Should().Contain(e1);
        all.Should().Contain(e2);
    }

    [Test]
    public async Task GetByStatusAsync_ReturnsCorrectEmergencies()
    {
        // Arrange
        var e1 = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var e2 = new Emergency(Guid.Empty, Guid.Empty, new Location(2, 2));
        e2.Resolve();
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> actives = await _repository.GetByStatusAsync(EmergencyStatus.Active);

        // Assert
        actives.Should().HaveCount(1);
        actives[0].Status.Should().Be(EmergencyStatus.Active);
    }

    [Test]
    public async Task GetActiveAsync_ReturnsOnlyActive()
    {
        // Arrange
        var e1 = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var e2 = new Emergency(Guid.Empty, Guid.Empty, new Location(3, 3));
        e2.Resolve();
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> actives = await _repository.GetActiveAsync();

        // Assert
        actives.Should().HaveCount(1);
        actives[0].Status.Should().Be(EmergencyStatus.Active);
    }

    [Test]
    public async Task GetByUserAsync_ReturnsEmergenciesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var e1 = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var e2 = new Emergency(Guid.Empty, Guid.Empty, new Location(4, 4));
        e1.AddResponse(userId, true);
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> byUser = await _repository.GetByUserAsync(userId);

        // Assert
        byUser.Should().NotBeNull();
        byUser.Should().HaveCount(1);
        byUser[0].Should().Be(e1);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergency()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        EmergencyStatus startingStatus = emergency.Status;
        await _repository.AddAsync(emergency);
        emergency.Resolve();

        // Act
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        startingStatus.Should().Be(EmergencyStatus.Active);
        loaded?.Status.Should().Be(EmergencyStatus.Resolved);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithResponse()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);
        const string Message = "I'm safe";

        // Act
        emergency.AddResponse(userId, true, Message);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.Responses.Should().HaveCount(1);
        loaded?.Responses[0].UserId.Should().Be(userId);
        loaded?.Responses[0].IsSafe.Should().BeTrue();
        loaded?.Responses[0].Message.Should().Be(Message);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithMultipleResponses()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(userId1, true, "Safe here");
        emergency.AddResponse(userId2, false, "Need help");
        await _repository.SaveAsync(emergency);
        var loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.Responses.Should().HaveCount(2);
        loaded?.Responses.Any(r => r.UserId == userId1 && r.IsSafe).Should().BeTrue();
        loaded?.Responses.Any(r => r.UserId == userId2 && !r.IsSafe).Should().BeTrue();
    }

    [Test]
    public async Task SaveAsync_UpdatesResolvedAtWhenEmergencyResolved()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        await _repository.AddAsync(emergency);
        DateTime beforeResolve = DateTime.UtcNow;

        // Act
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.ResolvedAt.Should().NotBeNull();
        loaded?.ResolvedAt.Should().BeOnOrAfter(beforeResolve);
        loaded?.Status.Should().Be(EmergencyStatus.Resolved);
    }

    [Test]
    public async Task SaveAsync_DoesNotUpdateResolvedAtWhenAlreadyResolved()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        await _repository.AddAsync(emergency);
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        DateTime? firstResolvedAt = (await _repository.GetByIdAsync(emergency.Id))!.ResolvedAt;

        // Act - Try to resolve again
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.ResolvedAt.Should().Be(firstResolvedAt);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithNullResponseMessage()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(userId, true, null!);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.Responses.Should().HaveCount(1);
        loaded?.Responses[0].Message.Should().BeEmpty();
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithEmptyResponseMessage()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(userId, false, "");
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.Responses.Should().HaveCount(1);
        loaded?.Responses[0].Message.Should().BeEmpty();
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyEmergencyTypePreserved()
    {
        // Arrange
        var originalType = EmergencyType.NaturalDisaster;
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone(), emergencyType: originalType);
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.EmergencyType.Should().Be(originalType);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyLocationPreserved()
    {
        // Arrange
        var originalLocation = new Location(10.5, 20.3);
        var emergency = new Emergency(Guid.Empty, Guid.Empty, originalLocation.Clone());
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        loaded?.Location.Latitude.Should().Be(originalLocation.Latitude);
        loaded?.Location.Longitude.Should().Be(originalLocation.Longitude);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyAffectedAreasPreserved()
    {
        // Arrange
        var location = new Location(15.0, 25.0);
        var area = new Area(location.Clone(), 50.0);
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location.Clone(), [area]);
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        loaded?.AffectedAreas.Should().HaveCount(1);
        loaded?.AffectedAreas[0].Should().Be(area);
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyCreatedAtPreserved()
    {
        // Arrange
        var emergency = new Emergency(Guid.Empty, Guid.Empty, TestLocation.Clone());
        await _repository.AddAsync(emergency);
        DateTime originalCreatedAt = emergency.CreatedAt;

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        loaded?.CreatedAt.Should().Be(originalCreatedAt);
    }
}
