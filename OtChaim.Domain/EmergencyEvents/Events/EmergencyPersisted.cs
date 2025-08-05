using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Event raised when an emergency is successfully persisted to the database.
/// </summary>
public class EmergencyPersisted(Guid emergencyId, DateTime persistedAt = default, Guid eventId = default) : DomainEventBase(persistedAt, eventId)
{
    /// <summary>
    /// The ID of the emergency that was persisted.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    
    /// <summary>
    /// The date and time when the emergency was persisted.
    /// </summary>
    public DateTime PersistedAt => DateTimeOccurredUtc;
} 