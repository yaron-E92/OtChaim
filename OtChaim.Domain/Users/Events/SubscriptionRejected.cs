using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Users.Events;

public class SubscriptionRejected : IEvent
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }
    public DateTime OccurredOn { get; }

    public SubscriptionRejected(Guid subscriberId, Guid subscribedToId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
        OccurredOn = DateTime.UtcNow;
    }
} 