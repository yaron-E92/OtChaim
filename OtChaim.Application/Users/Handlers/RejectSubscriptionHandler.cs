using System.Threading;
using System.Threading.Tasks;
using OtChaim.Application.Common;
using OtChaim.Application.Users.Commands;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events;

namespace OtChaim.Application.Users.Handlers;

public class RejectSubscriptionHandler : ICommandHandler<RejectSubscription>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventAggregator _eventAggregator;

    public RejectSubscriptionHandler(IUserRepository userRepository, IEventAggregator eventAggregator)
    {
        _userRepository = userRepository;
        _eventAggregator = eventAggregator;
    }

    public async Task Handle(RejectSubscription command, CancellationToken cancellationToken = default)
    {
        var subscriber = await _userRepository.GetByIdAsync(command.SubscriberId, cancellationToken);
        var subscribedTo = await _userRepository.GetByIdAsync(command.SubscribedToId, cancellationToken);

        // Publish the event through the event aggregator
        await _eventAggregator.PublishEventAsync(new SubscriptionRejected(command.SubscriberId, command.SubscribedToId), cancellationToken);
        
        await _userRepository.SaveAsync(subscriber, cancellationToken);
    }
} 