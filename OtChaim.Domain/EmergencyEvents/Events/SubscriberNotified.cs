using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class SubscriberNotified : DomainEventBase
{
    public Guid UserId { get; }
    public Guid SubscriberId { get; }
    public Guid EmergencyId { get; }
    public string? Message { get; }

    public SubscriberNotified(
        Guid userId,
        Guid emergencyId,
        Guid subscriberId,
        string? message = null,
        DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        UserId = userId;
        EmergencyId = emergencyId;
        SubscriberId = subscriberId;
        Message = message;
    }
}
