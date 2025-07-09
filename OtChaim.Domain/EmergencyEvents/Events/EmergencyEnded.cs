using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class EmergencyEnded : DomainEventBase
{
    public Guid EmergencyId { get; }
    public DateTime EndedOn => DateTimeOccurredUtc;

    public EmergencyEnded(Guid emergencyId, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        EmergencyId = emergencyId;
    }
}
