using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

/// <summary>
/// Handles the <see cref="StartEmergency"/> command by publishing an <see cref="EmergencyStarted"/> event.
/// </summary>
/// <param name="eventAggregator">The event aggregator to use for publishing events.</param>
public class StartEmergencyHandler(IEventAggregator eventAggregator) : ICommandHandler<StartEmergency>
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="StartEmergency"/> command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task Handle(StartEmergency command, CancellationToken cancellationToken = default)
    {
        var emergencyId = Guid.NewGuid();
        var emergencyStartedEvent = new EmergencyStarted(
            emergencyId,
            command.InitiatorUserId,
            command.Type,
            command.Location,
            command.AffectedAreas, command.Severity);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(emergencyStartedEvent, cancellationToken);
    }
}
