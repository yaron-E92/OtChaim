using System;
using OtChaim.Application.Common;

namespace OtChaim.Application.Users.Commands;

public class ApproveSubscription : ICommand
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }

    public ApproveSubscription(Guid subscriberId, Guid subscribedToId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
    }
} 