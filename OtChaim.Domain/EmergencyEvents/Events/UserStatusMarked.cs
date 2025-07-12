using Yaref92.Events;
using OtChaim.Domain.Users;

namespace OtChaim.Domain.EmergencyEvents.Events;

public class UserStatusMarked : DomainEventBase
{
    public Guid UserId { get; }
    public Guid EmergencyId { get; }
    public UserStatus Status { get; }
    public string Message { get; }

    public UserStatusMarked(
        Guid userId,
        Guid emergencyId,
        UserStatus status,
        string message = "",
        DateTime occurred = default, Guid eventId = default)
        : base(occurred, eventId)
    {
        UserId = userId;
        EmergencyId = emergencyId;
        Status = status;
        Message = message;
    }
}
