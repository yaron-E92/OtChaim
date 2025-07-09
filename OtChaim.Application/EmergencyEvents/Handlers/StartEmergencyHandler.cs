using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

public class StartEmergencyHandler(IEventAggregator eventAggregator) : ICommandHandler<StartEmergency>
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    public async Task Handle(StartEmergency command, CancellationToken cancellationToken = default)
    {
        var emergencySituationId = Guid.NewGuid();
        var emergencySituationStartedEvent = new EmergencyStarted(
            emergencySituationId,
            command.InitiatorUserId,
            command.Type,
            command.Location,
            command.Area,
            command.Severity);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(emergencySituationStartedEvent, cancellationToken);
    }
} 
