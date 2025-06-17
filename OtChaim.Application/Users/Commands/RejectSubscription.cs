using System;
using OtChaim.Application.Common;

namespace OtChaim.Application.Users.Commands;

public class RejectSubscription : ICommand
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }

    public RejectSubscription(Guid subscriberId, Guid subscribedToId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
    }
} 