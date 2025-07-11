using Microsoft.Extensions.DependencyInjection;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Application.EmergencyEvents.Handlers;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.IntegrationTests;

public class EmergencyIntegrationTests : IntegrationTestBase
{

    [Test]
    [Explicit("Integration test for starting an emergency.")]
    public async Task StartEmergencyHandler_CreatesEmergencyInDatabase()
    {
        // Arrange
        StartEmergencyHandler handler = Provider.GetRequiredService<StartEmergencyHandler>();
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(1.0, 2.0, "Test");
        var area = new Area(location, 100);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, area, Severity.High, "Test emergency");

        // Act
        await handler.Handle(command);
        IReadOnlyList<Emergency> emergencies = await repo.GetAllAsync();

        // Assert
        Assert.That(emergencies, Is.Not.Empty);
        Assert.That(emergencies.Any(e => e.Location.Equals(location) && e.Severity == Severity.High));
    }

    [Test]
    [Explicit("Integration test for marking user status in an emergency.")]
    public async Task MarkUserStatusHandler_AddsResponseToEmergency()
    {
        // Arrange
        StartEmergencyHandler startHandler = Provider.GetRequiredService<StartEmergencyHandler>();
        MarkUserStatusHandler markHandler = Provider.GetRequiredService<MarkUserStatusHandler>();
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(2.0, 3.0, "Test2");
        var area = new Area(location, 200);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, area, Severity.Medium, "Flood emergency");
        await startHandler.Handle(command);
        Emergency? emergency = (await repo.GetAllAsync())[0];
        const string Message = "I'm safe";
        var markCommand = new MarkUserStatus(Guid.NewGuid(), emergency.Id, UserStatus.Safe, Message);

        // Act
        await markHandler.Handle(markCommand);
        emergency = await repo.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(emergency.Responses, Is.Not.Empty);
        Assert.That(emergency.Responses.Any(r => r.Message == Message));
    }

    [Test]
    [Explicit("Integration test for ending an emergency.")]
    public async Task EndEmergencyHandler_ChangesEmergencyStatusToResolved()
    {
        // Arrange
        StartEmergencyHandler startHandler = Provider.GetRequiredService<StartEmergencyHandler>();
        EndEmergencyHandler endHandler = Provider.GetRequiredService<EndEmergencyHandler>();
        IEmergencyRepository repo = Provider.GetRequiredService<IEmergencyRepository>();
        var location = new Location(3.0, 4.0, "Test3");
        var area = new Area(location, 300);
        var command = new StartEmergency(Guid.NewGuid(), EmergencyType.NaturalDisaster, location, area, Severity.Low, "Earthquake");
        await startHandler.Handle(command);
        Emergency? emergency = (await repo.GetAllAsync())[0];
        var endCommand = new EndEmergency(emergency.Id);

        // Act
        await endHandler.Handle(endCommand);
        emergency = await repo.GetByIdAsync(emergency.Id);

        // Assert
        Assert.That(emergency.Status, Is.EqualTo(EmergencyStatus.Resolved));
    }
}
