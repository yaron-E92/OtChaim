using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Domain event raised when an emergency has been successfully persisted to the database.
/// This event signals that the emergency data has been safely stored and is available
/// for retrieval and processing by other parts of the system.
/// </summary>
/// <remarks>
/// The EmergencyPersisted event is raised after the EmergencyStarted event has been
/// processed and the emergency data has been successfully saved to the database.
/// This event is used to synchronize UI components and ensure that the emergency
/// dashboard and other views are updated to reflect the newly persisted emergency.
/// It provides a reliable way to confirm that emergency data has been stored
/// before updating the user interface.
/// </remarks>
public class EmergencyPersisted(Guid emergencyId, DateTime persistedAt = default, Guid eventId = default) : DomainEventBase(persistedAt, eventId)
{
    /// <summary>
    /// The unique identifier of the emergency that was persisted.
    /// </summary>
    /// <remarks>
    /// This ID corresponds to the EmergencyId from the EmergencyStarted event and
    /// can be used to retrieve the full emergency details from the database.
    /// It serves as a reference for UI updates and subsequent emergency operations.
    /// </remarks>
    public Guid EmergencyId { get; } = emergencyId;

    /// <summary>
    /// The date and time when the emergency was persisted to the database.
    /// </summary>
    /// <remarks>
    /// This timestamp indicates when the emergency data was successfully stored
    /// and can be used for audit trails, debugging, and performance monitoring.
    /// It inherits from the base DomainEventBase.DateTimeOccurredUtc property.
    /// </remarks>
    public DateTime PersistedAt => DateTimeOccurredUtc;
}