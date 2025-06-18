using System;
using NUnit.Framework;
using FluentAssertions;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Domain.Tests.EmergencyEvents;

[TestFixture]
public class EmergencyTests
{
    [Test]
    public void Create_WithValidInitiatorId_ShouldCreateActiveEmergency()
    {
        // Arrange
        var initiatorId = Guid.NewGuid();

        // Act
        var emergency = new Emergency(initiatorId);

        // Assert
        emergency.InitiatorId.Should().Be(initiatorId);
        emergency.Status.Should().Be(EmergencyStatus.Active);
        emergency.CreatedAt.Should().NotBe(default);
        emergency.ResolvedAt.Should().BeNull();
        emergency.Responses.Should().BeEmpty();
    }
}