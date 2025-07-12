using Yaref92.Events;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Event raised when a subscriber is notified about an emergency.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SubscriberNotified"/> class.
/// </remarks>
public class SubscriberNotified(
    Guid userId,
    Guid emergencyId,
    string message = "",
    DateTime occurred = default, Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// The ID of the user who was notified.
    /// </summary>
    public Guid UserId { get; } = userId;
    /// <summary>
    /// The ID of the emergency.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// The message sent to the subscriber, if any.
    /// </summary>
    public string Message { get; } = message;
}
