using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

/// <summary>
/// Handles the <see cref="EndEmergency"/> command by publishing an <see cref="EmergencyEnded"/> event.
/// </summary>
/// <param name="eventAggregator">The event aggregator to use for publishing events.</param>
public sealed class EndEmergencyHandler(IEventAggregator eventAggregator) : ICommandHandler<EndEmergency>
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="EndEmergency"/> command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task Handle(EndEmergency command, CancellationToken cancellationToken = default)
    {
        var emergencyEndedEvent = new EmergencyEnded(command.EmergencyId);
        await _eventAggregator.PublishEventAsync(emergencyEndedEvent, cancellationToken);
    }
}
