using OtChaim.Application.Common;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.Handlers;

/// <summary>
/// Handles the <see cref="MarkUserStatus"/> command by publishing a <see cref="UserStatusMarked"/> event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MarkUserStatusHandler"/> class.
/// </remarks>
/// <param name="eventAggregator">The event aggregator to use for publishing events.</param>
public sealed class MarkUserStatusHandler(IEventAggregator eventAggregator) : ICommandHandler<MarkUserStatus>
{
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="MarkUserStatus"/> command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task Handle(MarkUserStatus command, CancellationToken cancellationToken = default)
    {
        var userStatusMarkedEvent = new UserStatusMarked(
            command.UserId,
            command.EmergencyId,
            command.Status,
            command.Message);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(userStatusMarkedEvent, cancellationToken);
    }
}
