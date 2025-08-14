using OtChaim.Domain.Common;
using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Domain event raised when a new emergency is started by a user.
/// This event signals the beginning of an emergency situation and contains all
/// the essential information needed to process and respond to the emergency.
/// </summary>
/// <remarks>
/// The EmergencyStarted event is the primary domain event for emergency management.
/// It is published immediately when a user initiates an emergency and contains
/// comprehensive information including location, affected areas, attachments,
/// and contact preferences. This event triggers the emergency response workflow
/// and notifies all relevant subscribers about the new emergency situation.
/// </remarks>
public class EmergencyStarted(
    Guid emergencyId,
    Guid initiatorUserId,
    EmergencyType type,
    Location location,
    IEnumerable<Area> affectedAreas,
    string description = "",
    EmergencyAttachments? attachments = null,
    DateTime occurredAt = default,
    Guid eventId = default) : DomainEventBase(occurredAt, eventId)
{
    /// <summary>
    /// The unique identifier of the emergency that was started.
    /// </summary>
    /// <remarks>
    /// This ID is used to track the emergency throughout its lifecycle and
    /// is referenced by other events and commands related to this emergency.
    /// </remarks>
    public Guid EmergencyId { get; } = emergencyId;

    /// <summary>
    /// The unique identifier of the user who initiated the emergency.
    /// </summary>
    /// <remarks>
    /// This ID identifies the person who started the emergency and may be used
    /// for contact purposes, status updates, and emergency response coordination.
    /// </remarks>
    public Guid InitiatorUserId { get; } = initiatorUserId;

    /// <summary>
    /// The type of emergency that was started.
    /// </summary>
    /// <remarks>
    /// The emergency type determines the appropriate response protocol, priority level,
    /// and may affect which emergency services are contacted.
    /// </remarks>
    public EmergencyType Type { get; } = type;

    /// <summary>
    /// The location where the emergency occurred.
    /// </summary>
    /// <remarks>
    /// The location contains geographical coordinates and a human-readable description
    /// that helps emergency responders locate the incident quickly and accurately.
    /// </remarks>
    public Location Location { get; } = location;

    /// <summary>
    /// The areas affected by the emergency.
    /// </summary>
    /// <remarks>
    /// Affected areas define the geographical scope of the emergency and may include
    /// evacuation zones, areas requiring special attention, or regions that need
    /// to be notified about the emergency situation.
    /// </remarks>
    public IEnumerable<Area> AffectedAreas { get; } = affectedAreas;

    /// <summary>
    /// The description of the emergency provided by the user.
    /// </summary>
    /// <remarks>
    /// This description provides additional context about the emergency situation,
    /// including symptoms, circumstances, or specific details that help emergency
    /// responders understand the nature and severity of the situation.
    /// </remarks>
    public string Description { get; } = description;

    /// <summary>
    /// The attachments associated with the emergency.
    /// </summary>
    /// <remarks>
    /// Attachments may include personal information, medical information, GPS location,
    /// pictures, documents, and contact preferences. These attachments provide
    /// additional context and information to emergency responders.
    /// </remarks>
    public EmergencyAttachments? Attachments { get; } = attachments;
    /// <summary>
    /// The date and time when the emergency started.
    /// </summary>
    public DateTime StartedOn => DateTimeOccurredUtc;
}
