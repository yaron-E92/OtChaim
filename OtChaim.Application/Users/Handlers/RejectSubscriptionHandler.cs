using OtChaim.Application.Common;
using OtChaim.Application.Users.Commands;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Users.Handlers;

/// <summary>
/// Handles the <see cref="RejectSubscription"/> command by publishing a <see cref="SubscriptionRejected"/> event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RejectSubscriptionHandler"/> class.
/// </remarks>
/// <param name="userRepository">The user repository to update.</param>
/// <param name="eventAggregator">The event aggregator to use for publishing events.</param>
public sealed class RejectSubscriptionHandler(IUserRepository userRepository, IEventAggregator eventAggregator) : ICommandHandler<RejectSubscription>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="RejectSubscription"/> command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task Handle(RejectSubscription command, CancellationToken cancellationToken = default)
    {
        User subscriber = await _userRepository.GetByIdAsync(command.SubscriberId, cancellationToken);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(new SubscriptionRejected(command.SubscriberId, command.SubscribedToId), cancellationToken);
        
        await _userRepository.SaveAsync(subscriber, cancellationToken);
    }
}
