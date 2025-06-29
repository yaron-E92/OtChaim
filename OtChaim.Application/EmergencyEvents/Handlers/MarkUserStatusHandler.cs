using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

public class MarkUserStatusHandler : ICommandHandler<MarkUserStatus>
{
    private readonly IEventAggregator _eventAggregator;

    public MarkUserStatusHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public async Task Handle(MarkUserStatus command, CancellationToken cancellationToken = default)
    {
        var userStatusMarkedEvent = new UserStatusMarked(
            command.UserId,
            command.EmergencySituationId,
            command.Status,
            command.Message);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(userStatusMarkedEvent, cancellationToken);
    }
} 
