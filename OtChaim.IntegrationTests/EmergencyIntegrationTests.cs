using Microsoft.Extensions.DependencyInjection;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Application.EmergencyEvents.Handlers;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;
using FluentAssertions;
using OtChaim.Application.Common;

namespace OtChaim.IntegrationTests;

public class EmergencyIntegrationTests : IntegrationTestBase
{

    [Test]
    [Explicit("Integration test for starting an emergency.")]
    [Category("Integration")]
    public async Task StartEmergencyHandler_CreatesEmergencyInDatabase()
    {
        // Arrange
        StartEmergencyHandler handler = Provider.GetRequiredService<ICommandHandler<StartEmergency>>() as StartEmergencyHandler;
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(1.0, 2.0, "Test");
        var area = new Area(location, 100);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, [area], Severity.High, "Test emergency");

        // Act
        await handler.Handle(command);
        IReadOnlyList<Emergency> emergencies = await repo.GetAllAsync();

        // Assert
        emergencies.Should().NotBeEmpty();
        emergencies.Any(e => e.Location.Equals(location) && e.Severity == Severity.High).Should().BeTrue();
    }

    [Test]
    [Explicit("Integration test for marking user status in an emergency.")]
    [Category("Integration")]
    public async Task MarkUserStatusHandler_AddsResponseToEmergency()
    {
        // Arrange
        StartEmergencyHandler startHandler = Provider.GetRequiredService<ICommandHandler<StartEmergency>>() as StartEmergencyHandler;
        MarkUserStatusHandler markHandler = Provider.GetRequiredService<ICommandHandler<MarkUserStatus>>() as MarkUserStatusHandler;
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(2.0, 3.0, "Test2");
        var area = new Area(location, 200);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, [area], Severity.Medium, "Flood emergency");
        await startHandler.Handle(command);
        Emergency? emergency = (await repo.GetAllAsync())[0];
        const string Message = "I'm safe";
        var markCommand = new MarkUserStatus(Guid.NewGuid(), emergency.Id, UserStatus.Safe, Message);

        // Act
        await markHandler.Handle(markCommand);
        emergency = await repo.GetByIdAsync(emergency.Id);

        // Assert
        emergency?.Responses.Should().NotBeEmpty();
        emergency?.Responses.Any(r => r.Message == Message).Should().BeTrue();
    }

    [Test]
    [Explicit("Integration test for ending an emergency.")]
    [Category("Integration")]
    public async Task EndEmergencyHandler_ChangesEmergencyStatusToResolved()
    {
        // Arrange
        StartEmergencyHandler startHandler = Provider.GetRequiredService<ICommandHandler<StartEmergency>>() as StartEmergencyHandler;
        EndEmergencyHandler endHandler = Provider.GetRequiredService<ICommandHandler<EndEmergency>>() as EndEmergencyHandler;
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(3.0, 4.0, "Test3");
        var area = new Area(location, 300);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, [area], Severity.Low, "Earthquake");
        await startHandler.Handle(command);
        Emergency? emergency = (await repo.GetAllAsync())[0];
        var endCommand = new EndEmergency(emergency.Id);

        // Act
        await endHandler.Handle(endCommand);
        emergency = await repo.GetByIdAsync(emergency.Id);

        // Assert
        emergency?.Status.Should().Be(EmergencyStatus.Resolved);
    }
}
