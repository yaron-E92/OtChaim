using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Users.EventSubscribers;

/// <summary>
/// Subscribes to user subscription-related domain events and updates the user repository accordingly.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SubscriptionEventSubscriber"/> class.
/// </remarks>
/// <param name="userRepository">The user repository to update.</param>
public class SubscriptionEventSubscriber(IUserRepository userRepository) :
    IAsyncEventSubscriber<SubscriptionRequested>,
    IAsyncEventSubscriber<SubscriptionApproved>,
    IAsyncEventSubscriber<SubscriptionRejected>
{
    private readonly IUserRepository _userRepository = userRepository;

    /// <summary>
    /// Handles the <see cref="SubscriptionRequested"/> event by updating the user repository.
    /// </summary>
    public async Task OnNextAsync(SubscriptionRequested @event, CancellationToken cancellationToken = default)
    {
        User subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionRequested(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }

    /// <summary>
    /// Handles the <see cref="SubscriptionApproved"/> event by updating the user repository.
    /// </summary>
    public async Task OnNextAsync(SubscriptionApproved @event, CancellationToken cancellationToken = default)
    {
        User subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionApproved(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }

    /// <summary>
    /// Handles the <see cref="SubscriptionRejected"/> event by updating the user repository.
    /// </summary>
    public async Task OnNextAsync(SubscriptionRejected @event, CancellationToken cancellationToken = default)
    {
        User subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionRejected(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }
}
