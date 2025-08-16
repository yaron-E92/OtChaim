using FluentAssertions;
using NSubstitute;
using OtChaim.Application.EmergencyEvents.EventSubscribers;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Tests.Emergencies.EventSubscribers;

[TestFixture]
public class EmergencyEventSubscriberTests
{
    private IEmergencyRepository _repo;
    private IEventAggregator _eventAggregator;

    [SetUp]
    public void Setup()
    {
        _repo = Substitute.For<IEmergencyRepository>();
        _eventAggregator = Substitute.For<IEventAggregator>();
    }

    [Test]
    public async Task OnNextAsync_EmergencyStarted_CreatesAndPersistsEmergency()
    {
        // Arrange
        EmergencyEventSubscriber subscriber = new(_repo, _eventAggregator);
        Location location = new(1, 2);
        Area area = new(location, 100);
        EmergencyStarted evt = new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            EmergencyType.WeatherAlert,
            location,
            [area],
            "", null,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        await _repo.Received(1).AddAsync(Arg.Is<Emergency>(e =>
            e.Location.Equals(evt.Location) &&
            e.EmergencyType == evt.Type &&
            e.AffectedAreas.SequenceEqual(evt.AffectedAreas)
        ), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_EmergencyEnded_ResolvesEmergency()
    {
        // Arrange
        Guid emergencyId = Guid.NewGuid();
        var emergency = new Emergency(emergencyId, Guid.NewGuid(), new Location(1, 2));
        _repo.GetByIdAsync(emergencyId, Arg.Any<CancellationToken>()).Returns(emergency);
        var subscriber = new EmergencyEventSubscriber(_repo, _eventAggregator);
        var evt = new EmergencyEnded(emergencyId, DateTime.UtcNow, Guid.NewGuid());

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        emergency.Status.Should().Be(EmergencyStatus.Resolved);
        await _repo.Received(1).SaveAsync(emergency, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_UserStatusMarked_AddsResponse()
    {
        // Arrange
        Guid emergencyId = Guid.NewGuid();
        var emergency = new Emergency(emergencyId, Guid.NewGuid(), new Location(1, 2));
        _repo.GetByIdAsync(emergencyId, Arg.Any<CancellationToken>()).Returns(emergency);
        var subscriber = new EmergencyEventSubscriber(_repo, _eventAggregator);
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
            r.IsSafe &&
            r.Message == evt.Message
        );
        await _repo.Received(1).SaveAsync(emergency, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_EmergencyEnded_DoesNothingIfEmergencyNotFound()
    {
        // Arrange
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Emergency?)null);
        var subscriber = new EmergencyEventSubscriber(_repo, _eventAggregator);
        var evt = new EmergencyEnded(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid());

        // Act
        await subscriber.OnNextAsync(evt);

        // Assert
        await _repo.DidNotReceive().SaveAsync(Arg.Any<Emergency>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task OnNextAsync_UserStatusMarked_DoesNothingIfEmergencyNotFound()
    {
        // Arrange
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Emergency?)null);
        var subscriber = new EmergencyEventSubscriber(_repo, _eventAggregator);
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
        await _repo.DidNotReceive().SaveAsync(Arg.Any<Emergency>(), Arg.Any<CancellationToken>());
    }
}
