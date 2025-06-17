using System.Threading;
using System.Threading.Tasks;
using OtChaim.Application.Common;
using OtChaim.Application.Users.Commands;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;

namespace OtChaim.Application.Users.Handlers;

public class RequestSubscriptionHandler : ICommandHandler<RequestSubscription>
{
    private readonly IUserRepository _userRepository;

    public RequestSubscriptionHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(RequestSubscription command, CancellationToken cancellationToken = default)
    {
        var subscriber = await _userRepository.GetByIdAsync(command.SubscriberId, cancellationToken);
        var subscribedTo = await _userRepository.GetByIdAsync(command.SubscribedToId, cancellationToken);

        subscriber.RaiseEvent(new SubscriptionRequested(command.SubscriberId, command.SubscribedToId));
        
        await _userRepository.SaveAsync(subscriber, cancellationToken);
    }
} 