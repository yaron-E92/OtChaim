using OtChaim.Application.Common;
using OtChaim.Domain.Users;

namespace OtChaim.Application.EmergencyEvents.Commands;

/// <summary>
/// Command to mark a user's status in an emergency event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MarkUserStatus"/> class.
/// </remarks>
/// <param name="userId">The ID of the user whose status is being marked.</param>
/// <param name="emergencyId">The ID of the emergency event.</param>
/// <param name="status">The status of the user.</param>
/// <param name="message">An optional message from the user.</param>
public class MarkUserStatus(
    Guid userId,
    Guid emergencyId,
    UserStatus status,
    string message = "") : ICommand
{

    /// <summary>
    /// The ID of the user whose status is being marked.
    /// </summary>
    public Guid UserId { get; } = userId;
    /// <summary>
    /// The ID of the emergency event.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// The status of the user.
    /// </summary>
    public UserStatus Status { get; } = status;
    /// <summary>
    /// An optional message from the user.
    /// </summary>
    public string Message { get; } = message;
}
