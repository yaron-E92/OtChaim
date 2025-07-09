using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

public class EndEmergencyHandler(IEventAggregator eventAggregator) : ICommandHandler<EndEmergency>
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    public async Task Handle(EndEmergency command, CancellationToken cancellationToken = default)
    {
        var emergencyEndedEvent = new EmergencyEnded(command.EmergencyId);
        await _eventAggregator.PublishEventAsync(emergencyEndedEvent, cancellationToken);
    }
} 
