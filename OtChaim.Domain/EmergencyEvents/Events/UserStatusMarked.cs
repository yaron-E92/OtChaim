using Yaref92.Events;
using OtChaim.Domain.Users;

namespace OtChaim.Domain.EmergencyEvents.Events;

/// <summary>
/// Event raised when a user's status is marked in an emergency.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UserStatusMarked"/> class.
/// </remarks>
/// <param name="userId">The ID of the user whose status was marked</param>
/// <param name="emergencyId">The ID of the emergency</param>
/// <param name="status">The status of the user</param>
public class UserStatusMarked(
    Guid userId,
    Guid emergencyId,
    UserStatus status,
    string message = "",
    DateTime occurred = default, Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// The ID of the user whose status was marked.
    /// </summary>
    public Guid UserId { get; } = userId;
    /// <summary>
    /// The ID of the emergency.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// The status of the user.
    /// </summary>
    public UserStatus Status { get; } = status;
    /// <summary>
    /// The message provided by the user.
    /// </summary>
    public string Message { get; } = message;
}
