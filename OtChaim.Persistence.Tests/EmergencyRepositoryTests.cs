using Microsoft.EntityFrameworkCore;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

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
        var emergency = new Emergency(TestLocation.Clone());

        // Act
        await _repository.AddAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(loaded, Is.Not.Null);
        Assert.That(loaded.Id, Is.EqualTo(emergency.Id));
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllEmergencies()
    {
        // Arrange
        var e1 = new Emergency(TestLocation.Clone());
        var e2 = new Emergency(new Location(1, 1), null, Severity.High);
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> all = await _repository.GetAllAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(all, Has.Count.EqualTo(2));
            Assert.That(all, Has.Member(e1));
            Assert.That(all, Has.Member(e2));
        });
    }

    [Test]
    public async Task GetByStatusAsync_ReturnsCorrectEmergencies()
    {
        // Arrange
        var e1 = new Emergency(TestLocation.Clone());
        var e2 = new Emergency(new Location(2, 2), null, Severity.Low);
        e2.Resolve();
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> actives = await _repository.GetByStatusAsync(EmergencyStatus.Active);

        // Assert
        Assert.That(actives, Has.Count.EqualTo(1));
        Assert.That(actives[0].Status, Is.EqualTo(EmergencyStatus.Active));
    }

    [Test]
    public async Task GetActiveAsync_ReturnsOnlyActive()
    {
        // Arrange
        var e1 = new Emergency(TestLocation.Clone());
        var e2 = new Emergency(new Location(3, 3), null, Severity.Medium);
        e2.Resolve();
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> actives = await _repository.GetActiveAsync();

        // Assert
        Assert.That(actives, Has.Count.EqualTo(1));
        Assert.That(actives[0].Status, Is.EqualTo(EmergencyStatus.Active));
    }

    [Test]
    public async Task GetByUserAsync_ReturnsEmergenciesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var e1 = new Emergency(TestLocation.Clone());
        var e2 = new Emergency(new Location(4, 4));
        e1.AddResponse(userId, true);
        await _repository.AddAsync(e1);
        await _repository.AddAsync(e2);

        // Act
        IReadOnlyList<Emergency> byUser = await _repository.GetByUserAsync(userId);

        // Assert
        Assert.That(byUser, Is.Not.Null);
        Assert.That(byUser, Has.Count.EqualTo(1));
        Assert.That(byUser[0], Is.EqualTo(e1));
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergency()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        EmergencyStatus startingStatus = emergency.Status;
        await _repository.AddAsync(emergency);
        emergency.Resolve();

        // Act
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(startingStatus, Is.EqualTo(EmergencyStatus.Active));
            Assert.That(loaded?.Status, Is.EqualTo(EmergencyStatus.Resolved));
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithResponse()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);
        const string Message = "I'm safe";

        // Act
        emergency.AddResponse(userId, true, Message);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loaded?.Responses, Has.Count.EqualTo(1));
            Assert.That(loaded?.Responses[0].UserId, Is.EqualTo(userId));
            Assert.That(loaded?.Responses[0].IsSafe, Is.True);
            Assert.That(loaded?.Responses[0].Message, Is.EqualTo(Message));
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithMultipleResponses()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        await _repository.AddAsync(emergency);
        
        // Act
        emergency.AddResponse(userId1, true, "Safe here");
        emergency.AddResponse(userId2, false, "Need help");
        await _repository.SaveAsync(emergency);
        var loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loaded?.Responses, Has.Count.EqualTo(2));
            Assert.That(loaded?.Responses.Any(r => r.UserId == userId1 && r.IsSafe), Is.True);
            Assert.That(loaded?.Responses.Any(r => r.UserId == userId2 && !r.IsSafe), Is.True);
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesResolvedAtWhenEmergencyResolved()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        await _repository.AddAsync(emergency);
        DateTime beforeResolve = DateTime.UtcNow;
        
        // Act
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loaded?.ResolvedAt, Is.Not.Null);
            Assert.That(loaded?.ResolvedAt, Is.GreaterThanOrEqualTo(beforeResolve));
            Assert.That(loaded?.Status, Is.EqualTo(EmergencyStatus.Resolved));
        });
    }

    [Test]
    public async Task SaveAsync_DoesNotUpdateResolvedAtWhenAlreadyResolved()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        await _repository.AddAsync(emergency);
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        DateTime? firstResolvedAt = (await _repository.GetByIdAsync(emergency.Id))!.ResolvedAt;
        
        // Act - Try to resolve again
        emergency.Resolve();
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(loaded?.ResolvedAt, Is.EqualTo(firstResolvedAt));
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithNullResponseMessage()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);
        
        // Act
        emergency.AddResponse(userId, true, null!);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loaded?.Responses, Has.Count.EqualTo(1));
            Assert.That(loaded?.Responses[0].Message, Is.Empty);
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyWithEmptyResponseMessage()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        var userId = Guid.NewGuid();
        await _repository.AddAsync(emergency);
        
        // Act
        emergency.AddResponse(userId, false, "");
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(loaded?.Responses, Has.Count.EqualTo(1));
            Assert.That(loaded?.Responses[0].Message, Is.EqualTo(""));
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencySeverityPreserved()
    {
        // Arrange
        Severity originalSeverity = Severity.Low;

        var emergency = new Emergency(TestLocation.Clone(), severity: originalSeverity);
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(loaded?.Severity, Is.EqualTo(originalSeverity));
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyEmergencyTypePreserved()
    {
        // Arrange
        var originalType = EmergencyType.NaturalDisaster;
        var emergency = new Emergency(TestLocation.Clone(), emergencyType: originalType);
        await _repository.AddAsync(emergency);

        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        var loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(loaded.EmergencyType, Is.EqualTo(originalType));
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyLocationPreserved()
    {
        // Arrange
        var originalLocation = new Location(10.5, 20.3);
        var emergency = new Emergency(originalLocation.Clone());
        await _repository.AddAsync(emergency);
        
        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(loaded?.Location.Latitude, Is.EqualTo(originalLocation.Latitude));
            Assert.That(loaded?.Location.Longitude, Is.EqualTo(originalLocation.Longitude));
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyAffectedAreasPreserved()
    {
        // Arrange
        var location = new Location(15.0, 25.0);
        var area = new Area(location.Clone(), 50.0);
        var emergency = new Emergency(location.Clone(), [area]);
        await _repository.AddAsync(emergency);
        
        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(loaded?.AffectedAreas, Has.Count.EqualTo(1));
            Assert.That(loaded?.AffectedAreas[0], Is.EqualTo(area));
        });
    }

    [Test]
    public async Task SaveAsync_UpdatesEmergencyCreatedAtPreserved()
    {
        // Arrange
        var emergency = new Emergency(TestLocation.Clone());
        await _repository.AddAsync(emergency);
        DateTime originalCreatedAt = emergency.CreatedAt;
        
        // Act
        emergency.AddResponse(Guid.NewGuid(), true);
        await _repository.SaveAsync(emergency);
        Emergency? loaded = await _repository.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(loaded?.CreatedAt, Is.EqualTo(originalCreatedAt));
    }
} 
