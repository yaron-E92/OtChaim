using System;
using OtChaim.Application.Common;

namespace OtChaim.Application.Users.Commands;

public class RequestSubscription : ICommand
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }

    public RequestSubscription(Guid subscriberId, Guid subscribedToId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
    }
}