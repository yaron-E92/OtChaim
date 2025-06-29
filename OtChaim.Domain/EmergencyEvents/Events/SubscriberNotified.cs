using Yaref92.Events.Abstractions;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class SubscriberNotified : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid SubscriberId { get; }
        public Guid EmergencyId { get; }
        public string? Message { get; }
        public DateTime DateTimeOccurredUtc { get; }

        public SubscriberNotified(
            Guid userId,
            Guid emergencyId,
            Guid subscriberId,
            string? message = null)
        {
            UserId = userId;
            EmergencyId = emergencyId;
            SubscriberId = subscriberId;
            Message = message;
            DateTimeOccurredUtc = DateTime.UtcNow;
        }
    }
}
