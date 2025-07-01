using Yaref92.Events.Abstractions;

namespace OtChaim.Domain.Users.Events;

public class SubscriptionRejected : IDomainEvent
{
    public Guid SubscriberId { get; }
    public Guid SubscribedToId { get; }
    public DateTime DateTimeOccurredUtc { get; }

    public SubscriptionRejected(Guid subscriberId, Guid subscribedToId)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
        DateTimeOccurredUtc = DateTime.UtcNow;
    }
} 
