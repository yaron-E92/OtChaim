using Yaref92.Events;

namespace OtChaim.Domain.Users.Events;

public class SubscriptionRejected : DomainEventBase
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }

    public SubscriptionRejected(Guid subscriberId, Guid subscribedToId, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
    }
} 
