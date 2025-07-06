using OtChaim.Domain.Common;
using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class EmergencySituationStarted : DomainEventBase
{
    public Guid EmergencySituationId { get; }
    public Guid InitiatorId { get; }
    public EmergencyType EmergencyType { get; }
    public Location Location { get; }
    public Area AffectedArea { get; }
    public DateTime StartedOn => DateTimeOccurredUtc;

    public EmergencySituationStarted(
        Guid emergencySituationId,
        Guid initiatorId,
        EmergencyType emergencyType,
        Location location,
        Area affectedArea, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        EmergencySituationId = emergencySituationId;
        InitiatorId = initiatorId;
        EmergencyType = emergencyType;
        Location = location;
        AffectedArea = affectedArea;
    }
}
