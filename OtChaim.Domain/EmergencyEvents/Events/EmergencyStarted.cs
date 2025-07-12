using OtChaim.Domain.Common;
using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Event raised when an emergency is started.
/// </summary>
public class EmergencyStarted(Guid emergencyId, Guid initiatorId, EmergencyType emergencyType,
                              Location location, IEnumerable<Area> affectedAreas, Severity severity,
                              DateTime occurred = default, Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// Gets the ID of the emergency.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// Gets the ID of the user who initiated the emergency.
    /// </summary>
    public Guid InitiatorId { get; } = initiatorId;
    /// <summary>
    /// Gets the type of the emergency.
    /// </summary>
    public EmergencyType EmergencyType { get; } = emergencyType;
    /// <summary>
    /// Gets the location of the emergency.
    /// </summary>
    public Location Location { get; } = location;
    /// <summary>
    /// Gets the affected area of the emergency.
    /// </summary>
    public IEnumerable<Area> AffectedAreas { get; } = affectedAreas;
    /// <summary>
    /// Gets the severity of the emergency.
    /// </summary>
    public Severity Severity { get; } = severity;
    /// <summary>
    /// Gets the date and time when the emergency started.
    /// </summary>
    public DateTime StartedOn => DateTimeOccurredUtc;
}
