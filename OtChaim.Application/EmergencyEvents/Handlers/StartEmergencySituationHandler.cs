using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

public class StartEmergencySituationHandler : ICommandHandler<StartEmergencySituation>
{
    private readonly IEventAggregator _eventAggregator;

    public StartEmergencySituationHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public async Task Handle(StartEmergencySituation command, CancellationToken cancellationToken = default)
    {
        var emergencySituationId = Guid.NewGuid();
        var emergencySituationStartedEvent = new EmergencySituationStarted(
            emergencySituationId,
            command.InitiatorUserId,
            command.Type,
            command.Location,
            command.Area);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(emergencySituationStartedEvent, cancellationToken);
    }
} 
