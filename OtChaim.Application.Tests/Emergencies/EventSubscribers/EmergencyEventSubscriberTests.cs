using NSubstitute;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Application.EmergencyEvents.EventSubscribers;
using OtChaim.Domain.Users;
using OtChaim.Domain.Common;
using FluentAssertions;

namespace OtChaim.Application.Tests.Emergencies.EventSubscribers;

[TestFixture]
public class EmergencyEventSubscriberTests
{
    [Test]
    public async Task OnNextAsync_EmergencyStarted_CreatesAndPersistsEmergency()
    {
        // Arrange
        IEmergencyRepository repo = Substitute.For<IEmergencyRepository>();
        EmergencyEventSubscriber subscriber = new(repo);
        Location location = new(1, 2);
        Area area = new(location, 100);
        EmergencyStarted evt = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            EmergencyType.WeatherAlert,
            location,
            [area], Severity.High,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        await repo.Received(1).AddAsync(Arg.Is<Emergency>(e =>
            e.Location == evt.Location &&
            e.EmergencyType == evt.EmergencyType &&
            e.AffectedAreas.SequenceEqual(evt.AffectedAreas)
        ), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_EmergencyEnded_ResolvesEmergency()
    {
        // Arrange
        IEmergencyRepository repo = Substitute.For<IEmergencyRepository>();
        Guid emergencyId = Guid.NewGuid();
        var emergency = new Emergency(emergencyId, Guid.NewGuid(), new Location(1, 2));
        repo.GetByIdAsync(emergencyId, Arg.Any<CancellationToken>()).Returns(emergency);
        var subscriber = new EmergencyEventSubscriber(repo);
        var evt = new EmergencyEnded(emergencyId, DateTime.UtcNow, Guid.NewGuid());

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        emergency.Status.Should().Be(EmergencyStatus.Resolved);
        await repo.Received(1).SaveAsync(emergency, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_UserStatusMarked_AddsResponse()
    {
        // Arrange
        IEmergencyRepository repo = Substitute.For<IEmergencyRepository>();
        Guid emergencyId = Guid.NewGuid();
        var emergency = new Emergency(emergencyId, Guid.NewGuid(), new Location(1, 2));
        repo.GetByIdAsync(emergencyId, Arg.Any<CancellationToken>()).Returns(emergency);
        var subscriber = new EmergencyEventSubscriber(repo);
        var evt = new UserStatusMarked(
            Guid.NewGuid(),
            emergencyId,
            UserStatus.Safe,
            "I'm safe!",
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        emergency.Responses.Should().ContainSingle(r =>
            r.UserId == evt.UserId &&
            r.IsSafe == true &&
            r.Message == evt.Message
        );
        await repo.Received(1).SaveAsync(emergency, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_EmergencyEnded_DoesNothingIfEmergencyNotFound()
    {
        // Arrange
        IEmergencyRepository repo = Substitute.For<IEmergencyRepository>();
        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Emergency?)null);
        var subscriber = new EmergencyEventSubscriber(repo);
        var evt = new EmergencyEnded(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid());

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        await repo.DidNotReceive().SaveAsync(Arg.Any<Emergency>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_UserStatusMarked_DoesNothingIfEmergencyNotFound()
    {
        // Arrange
        IEmergencyRepository repo = Substitute.For<IEmergencyRepository>();
        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Emergency?)null);
        var subscriber = new EmergencyEventSubscriber(repo);
        var evt = new UserStatusMarked(
            Guid.NewGuid(),
            Guid.NewGuid(),
            UserStatus.Safe,
            "I'm safe!",
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        await repo.DidNotReceive().SaveAsync(Arg.Any<Emergency>(), Arg.Any<CancellationToken>());
    }
}
