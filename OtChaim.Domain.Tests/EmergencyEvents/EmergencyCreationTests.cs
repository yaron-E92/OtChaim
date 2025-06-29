using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Tests.EmergencyEvents;

[TestFixture]
public class EmergencyCreationTests
{
    [Test]
    public void CannotCreateEmergency_WithoutLocation()
    {
        // Act
        Action act = () => new Emergency(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void CanCreateEmergency_WithLocationOnly()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818); // Tel Aviv coords

        // Act
        var emergency = new Emergency(location);

        // Assert
        emergency.Location.Should().Be(location);
        emergency.AffectedAreas.Should().NotBeNull();
        emergency.AffectedAreas.Should().HaveCount(1);
        emergency.CreatedAt.Should().NotBe(default);
        emergency.Status.Should().Be(EmergencyStatus.Active);
        emergency.Severity.Should().Be(Severity.Medium);
        emergency.EmergencyType.Should().BeNull();
    }

    [Test]
    public void CanCreateEmergency_WithLocationAndTypeAndSeverity()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);
        var type = EmergencyType.WeatherAlert;
        var severity = Severity.High;

        // Act
        var emergency = new Emergency(location, null, severity, type);

        // Assert
        emergency.Location.Should().Be(location);
        emergency.EmergencyType.Should().Be(type);
        emergency.Severity.Should().Be(severity);
        emergency.Status.Should().Be(EmergencyStatus.Active);
    }

    [Test]
    public void CreatingEmergency_RaisesEmergencyStartedEvent()
    {
        // Arrange
        var location = new Location(32.0853, 34.7818);

        // Act
        var emergency = new Emergency(location);

        // Assert
        // This will fail until event sourcing or domain event logic is implemented
        emergency.DomainEvents.Should().ContainSingle(e => e is EmergencyStarted);
    }
} 