using System;
using NUnit.Framework;
using FluentAssertions;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Tests.EmergencyEvents;

[TestFixture]
public class EmergencyTests
{
    [Test]
    public void CannotCreateEmergency_WithoutLocation()
    {
        // Act
        Action act = () => new Emergency(Guid.Empty, Guid.Empty, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Create_WithValidLocation_ShouldCreateActiveEmergency()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);

        // Act
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location);

        // Assert
        emergency.Location.Should().Be(location);
        emergency.AffectedAreas.Should().NotBeNull();
        emergency.AffectedAreas.Should().HaveCount(1);
        emergency.CreatedAt.Should().NotBe(default);
        emergency.Status.Should().Be(EmergencyStatus.Active);
        emergency.EmergencyType.Should().BeNull();
    }

    [Test]
    public void CanCreateEmergency_WithLocationAndType()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var type = EmergencyType.WeatherAlert;

        // Act
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location, null, type);

        // Assert
        emergency.Location.Should().Be(location);
        emergency.EmergencyType.Should().Be(type);
        emergency.Status.Should().Be(EmergencyStatus.Active);
    }

    [Test]
    public void Resolve_WhenActive_ShouldSetStatusToResolvedAndSetResolvedAt()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location);

        // Act
        emergency.Resolve();

        // Assert
        emergency.Status.Should().Be(EmergencyStatus.Resolved);
        emergency.ResolvedAt.Should().NotBeNull();
    }

    [Test]
    public void Resolve_WhenAlreadyResolved_ShouldNotChangeResolvedAt()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location);
        emergency.Resolve();
        var firstResolvedAt = emergency.ResolvedAt;

        // Act
        emergency.Resolve();

        // Assert
        emergency.Status.Should().Be(EmergencyStatus.Resolved);
        emergency.ResolvedAt.Should().Be(firstResolvedAt);
    }

    [Test]
    public void AddResponse_ShouldAddResponseToList()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var emergency = new Emergency(Guid.Empty, Guid.Empty, location);
        var userId = Guid.NewGuid();
        var isSafe = true;
        var message = "I'm safe!";

        // Act
        emergency.AddResponse(userId, isSafe, message);

        // Assert
        emergency.Responses.Should().ContainSingle(r => r.UserId == userId && r.IsSafe == isSafe && r.Message == message);
    }
}
