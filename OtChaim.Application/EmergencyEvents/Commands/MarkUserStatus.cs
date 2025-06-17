using OtChaim.Application.Common;
using OtChaim.Domain.Users;

namespace OtChaim.Application.EmergencyEvents.Commands;

public class MarkUserStatus : ICommand
{
    public Guid UserId { get; }
    public Guid EmergencySituationId { get; }
    public UserStatus Status { get; }
    public string Message { get; }

    public MarkUserStatus(
        Guid userId,
        Guid emergencySituationId,
        UserStatus status,
        string message = null)
    {
        UserId = userId;
        EmergencySituationId = emergencySituationId;
        Status = status;
        Message = message;
    }
}