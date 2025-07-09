using OtChaim.Domain.Common;
using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class EmergencyStarted(
    Guid emergencyId,
    Guid initiatorId,
    EmergencyType emergencyType,
    Location location,
    Area affectedArea,
    Severity severity,
    DateTime occurred = default,
    Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    public Guid EmergencyId { get; } = emergencyId;
    public Guid InitiatorId { get; } = initiatorId;
    public EmergencyType EmergencyType { get; } = emergencyType;
    public Location Location { get; } = location;
    public Area AffectedArea { get; } = affectedArea;
    public Severity Severity { get; } = severity;
    public DateTime StartedOn => DateTimeOccurredUtc;
}
