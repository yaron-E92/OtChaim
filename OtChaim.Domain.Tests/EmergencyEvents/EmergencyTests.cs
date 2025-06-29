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
    public void Create_WithValidLocation_ShouldCreateActiveEmergency()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);

        // Act
        var emergency = new Emergency(location);

        // Assert
        emergency.Location.Should().Be(location);
        emergency.Status.Should().Be(EmergencyStatus.Active);
        emergency.CreatedAt.Should().NotBe(default);
        emergency.ResolvedAt.Should().BeNull();
        emergency.Responses.Should().BeEmpty();
    }

    [Test]
    public void Resolve_WhenActive_ShouldSetStatusToResolvedAndSetResolvedAt()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var emergency = new Emergency(location);

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
        var emergency = new Emergency(location);
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
        var emergency = new Emergency(location);
        var userId = Guid.NewGuid();
        var isSafe = true;
        var message = "I'm safe!";

        // Act
        emergency.AddResponse(userId, isSafe, message);

        // Assert
        emergency.Responses.Should().ContainSingle(r => r.UserId == userId && r.IsSafe == isSafe && r.Message == message);
    }
}