using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Users.EventSubscribers;

public class SubscriptionEventSubscriber : 
    IAsyncEventSubscriber<SubscriptionRequested>,
    IAsyncEventSubscriber<SubscriptionApproved>,
    IAsyncEventSubscriber<SubscriptionRejected>
{
    private readonly IUserRepository _userRepository;

    public SubscriptionEventSubscriber(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnNextAsync(SubscriptionRequested @event, CancellationToken cancellationToken = default)
    {
        var subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionRequested(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }

    public async Task OnNextAsync(SubscriptionApproved @event, CancellationToken cancellationToken = default)
    {
        var subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionApproved(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }

    public async Task OnNextAsync(SubscriptionRejected @event, CancellationToken cancellationToken = default)
    {
        var subscribedTo = await _userRepository.GetByIdAsync(@event.SubscribedToId, cancellationToken);
        subscribedTo.OnSubscriptionRejected(@event);
        await _userRepository.SaveAsync(subscribedTo, cancellationToken);
    }
}
