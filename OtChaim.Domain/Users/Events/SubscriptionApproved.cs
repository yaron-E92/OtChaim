using Yaref92.Events.Abstractions;

namespace OtChaim.Domain.Users.Events
{
    public class SubscriptionApproved : IDomainEvent
    {
        public Guid SubscriberId { get; }
        public Guid SubscribedToId { get; }
        public DateTime DateTimeOccurredUtc { get; }

        public SubscriptionApproved(Guid subscriberId, Guid subscribedToId)
        {
            SubscriberId = subscriberId;
            SubscribedToId = subscribedToId;
            DateTimeOccurredUtc = DateTime.UtcNow;
        }
    }
} 
