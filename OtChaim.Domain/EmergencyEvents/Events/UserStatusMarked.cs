using Yaref92.Events.Abstractions;
using OtChaim.Domain.Users;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class UserStatusMarked : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid EmergencyId { get; }
        public UserStatus Status { get; }
        public string? Message { get; }
        public DateTime DateTimeOccurredUtc { get; }

        public UserStatusMarked(
            Guid userId,
            Guid emergencyId,
            UserStatus status,
            string? message = null)
        {
            UserId = userId;
            EmergencyId = emergencyId;
            Status = status;
            Message = message;
            DateTimeOccurredUtc = DateTime.UtcNow;
        }
    }
}
