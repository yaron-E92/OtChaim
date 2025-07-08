using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

public class StartEmergencyHandler : ICommandHandler<StartEmergency>
{
    private readonly IEventAggregator _eventAggregator;

    public StartEmergencyHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public async Task Handle(StartEmergency command, CancellationToken cancellationToken = default)
    {
        var emergencySituationId = Guid.NewGuid();
        var emergencySituationStartedEvent = new EmergencyStarted(
            emergencySituationId,
            command.InitiatorUserId,
            command.Type,
            command.Location,
            command.Area);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(emergencySituationStartedEvent, cancellationToken);
    }
} 
