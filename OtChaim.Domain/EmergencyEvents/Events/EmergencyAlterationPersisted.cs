using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Domain event raised when an emergency alteration has been successfully persisted to the database.
/// This event signals that the emergency modification has been safely stored and is available
/// for retrieval and processing by other parts of the system.
/// </summary>
/// <remarks>
/// The EmergencyAlterationPersisted event is raised after an EmergencyAltered event has been
/// processed and the emergency data has been successfully saved to the database. This event
/// is used to synchronize UI components and ensure that the emergency dashboard and other
/// views are updated to reflect the newly persisted emergency alteration.
/// 
/// This event provides a reliable way to confirm that emergency alteration data has been
/// stored before updating the user interface, preventing race conditions and ensuring
/// data consistency across the application.
/// </remarks>
public class EmergencyAlterationPersisted(Guid emergencyId, EmergencyAlterationType alterationType, DateTime persistedAt = default, Guid eventId = default) : DomainEventBase(persistedAt, eventId)
{
    /// <summary>
    /// The unique identifier of the emergency that was altered and persisted.
    /// </summary>
    /// <remarks>
    /// This ID corresponds to the EmergencyId from the EmergencyAltered event and
    /// can be used to retrieve the full emergency details from the database.
    /// It serves as a reference for UI updates and subsequent emergency operations.
    /// </remarks>
    public Guid EmergencyId { get; } = emergencyId;

    /// <summary>
    /// The type of alteration that was persisted to the database.
    /// </summary>
    /// <remarks>
    /// This property indicates what kind of change was successfully saved to the database,
    /// allowing UI components to react appropriately based on the type of persisted change.
    /// </remarks>
    public EmergencyAlterationType AlterationType { get; } = alterationType;

    /// <summary>
    /// The date and time when the emergency alteration was persisted to the database.
    /// </summary>
    /// <remarks>
    /// This timestamp indicates when the emergency alteration data was successfully stored
    /// and can be used for audit trails, debugging, and performance monitoring.
    /// It inherits from the base DomainEventBase.DateTimeOccurredUtc property.
    /// </remarks>
    public DateTime PersistedAt => DateTimeOccurredUtc;
} 