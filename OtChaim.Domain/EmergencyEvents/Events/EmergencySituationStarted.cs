using OtChaim.Domain.Common;
using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class EmergencyStarted : DomainEventBase
{
    public Guid EmergencyId { get; }
    public Guid InitiatorId { get; }
    public EmergencyType EmergencyType { get; }
    public Location Location { get; }
    public Area AffectedArea { get; }
    public DateTime StartedOn => DateTimeOccurredUtc;

    public EmergencyStarted(
        Guid emergencyId,
        Guid initiatorId,
        EmergencyType emergencyType,
        Location location,
        Area affectedArea, DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        EmergencyId = emergencyId;
        InitiatorId = initiatorId;
        EmergencyType = emergencyType;
        Location = location;
        AffectedArea = affectedArea;
    }
}
