using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class EmergencySituationEnded : DomainEventBase
{
    public Guid EmergencySituationId { get; }
    public DateTime EndedOn => DateTimeOccurredUtc;

    public EmergencySituationEnded(Guid emergencySituationId, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        EmergencySituationId = emergencySituationId;
    }
}
