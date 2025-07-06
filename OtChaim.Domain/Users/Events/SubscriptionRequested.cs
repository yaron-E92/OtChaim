using Yaref92.Events;

namespace OtChaim.Domain.Users.Events;

public class SubscriptionRequested : DomainEventBase
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }

    public SubscriptionRequested(Guid subscriberId, Guid subscribedToId, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
    }
}

