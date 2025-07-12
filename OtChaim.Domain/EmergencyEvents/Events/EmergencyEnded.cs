using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Event raised when an emergency is ended.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EmergencyEnded"/> class.
/// </remarks>
public class EmergencyEnded(Guid emergencyId, DateTime occurred = default,
                            Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// Gets the ID of the emergency.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// Gets the date and time when the emergency ended.
    /// </summary>
    public DateTime EndedOn => DateTimeOccurredUtc;
}
