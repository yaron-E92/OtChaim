using System.Threading;
using System.Threading.Tasks;
using OtChaim.Application.Common;
using OtChaim.Application.Users.Commands;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;

namespace OtChaim.Application.Users.Handlers;

public class ApproveSubscriptionHandler : ICommandHandler<ApproveSubscription>
{
    private readonly IUserRepository _userRepository;

    public ApproveSubscriptionHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(ApproveSubscription command, CancellationToken cancellationToken = default)
    {
        var subscriber = await _userRepository.GetByIdAsync(command.SubscriberId, cancellationToken);
        var subscribedTo = await _userRepository.GetByIdAsync(command.SubscribedToId, cancellationToken);

        subscriber.RaiseEvent(new SubscriptionApproved(command.SubscriberId, command.SubscribedToId));
        
        await _userRepository.SaveAsync(subscriber, cancellationToken);
    }
} 
