using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Domain event raised when an emergency alteration is requested or processed.
/// This event signals that an emergency modification has been initiated and is
/// being processed, but may not yet be persisted to the database.
/// </summary>
/// <remarks>
/// The EmergencyAltered event is raised when an emergency modification is requested,
/// such as status updates, resolution, edits, etc. This event occurs before the
/// actual database persistence and allows for immediate UI feedback and processing.
/// 
/// This event should be followed by an EmergencyAlterationPersisted event once
/// the alteration has been successfully saved to the database.
/// </remarks>
public class EmergencyAltered(Guid emergencyId, EmergencyAlterationType alterationType, DateTime alteredAt = default, Guid eventId = default) : DomainEventBase(alteredAt, eventId)
{
    /// <summary>
    /// The unique identifier of the emergency that was altered.
    /// </summary>
    /// <remarks>
    /// This ID corresponds to the EmergencyId and can be used to identify
    /// which emergency was modified.
    /// </remarks>
    public Guid EmergencyId { get; } = emergencyId;

    /// <summary>
    /// The type of alteration that was requested for the emergency.
    /// </summary>
    /// <remarks>
    /// This property indicates what kind of change was requested for the emergency,
    /// allowing UI components to react appropriately to different types of changes.
    /// </remarks>
    public EmergencyAlterationType AlterationType { get; } = alterationType;

    /// <summary>
    /// The date and time when the emergency alteration was requested.
    /// </summary>
    /// <remarks>
    /// This timestamp indicates when the emergency alteration was initiated
    /// and can be used for audit trails and processing timing.
    /// </remarks>
    public DateTime AlteredAt => DateTimeOccurredUtc;
}

/// <summary>
/// Enumeration of the different types of alterations that can occur to an emergency.
/// </summary>
/// <remarks>
/// This enum provides a way to categorize different types of emergency changes,
/// allowing UI components to react appropriately based on the type of modification.
/// </remarks>
public enum EmergencyAlterationType
{
    /// <summary>
    /// Emergency was created for the first time.
    /// </summary>
    Created,

    /// <summary>
    /// Emergency status was updated (e.g., Safe, HelpNeeded).
    /// </summary>
    StatusUpdated,

    /// <summary>
    /// Emergency was resolved/ended.
    /// </summary>
    Resolved,

    /// <summary>
    /// Emergency details were edited/modified.
    /// </summary>
    Edited,

    /// <summary>
    /// Emergency was assigned to responders.
    /// </summary>
    Assigned,

    /// <summary>
    /// Emergency location was updated.
    /// </summary>
    LocationUpdated,

    /// <summary>
    /// Emergency attachments were modified.
    /// </summary>
    AttachmentsModified,

    /// <summary>
    /// Emergency was escalated to higher priority.
    /// </summary>
    Escalated,

    /// <summary>
    /// Emergency was de-escalated to lower priority.
    /// </summary>
    DeEscalated,

    /// <summary>
    /// Emergency was cancelled/abandoned.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Emergency was reactivated after being resolved.
    /// </summary>
    Reactivated,

    /// <summary>
    /// Other type of alteration not covered by specific types.
    /// </summary>
    Other
} 