using OtChaim.Application.Common;
using OtChaim.Application.Users.Commands;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Users.Handlers;

/// <summary>
/// Handles the <see cref="ApproveSubscription"/> command by publishing a <see cref="SubscriptionApproved"/> event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApproveSubscriptionHandler"/> class.
/// </remarks>
/// <param name="userRepository">The user repository to update.</param>
/// <param name="eventAggregator">The event aggregator to use for publishing events.</param>
public sealed class ApproveSubscriptionHandler(IUserRepository userRepository, IEventAggregator eventAggregator) : ICommandHandler<ApproveSubscription>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="ApproveSubscription"/> command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public async Task Handle(ApproveSubscription command, CancellationToken cancellationToken = default)
    {
        User subscriber = await _userRepository.GetByIdAsync(command.SubscriberId, cancellationToken);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(new SubscriptionApproved(command.SubscriberId, command.SubscribedToId), cancellationToken);

        await _userRepository.SaveAsync(subscriber, cancellationToken);
    }
}
