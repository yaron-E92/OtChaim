using OtChaim.Application.Common;
using OtChaim.Domain.Users;

namespace OtChaim.Application.EmergencyEvents.Commands;

public class MarkUserStatus : ICommand
{
    public Guid UserId { get; }
    public Guid EmergencyId { get; }
    public UserStatus Status { get; }
    public string Message { get; }

    public MarkUserStatus(
        Guid userId,
        Guid emergencyId,
        UserStatus status,
        string message = null)
    {
        UserId = userId;
        EmergencyId = emergencyId;
        Status = status;
        Message = message;
    }
}