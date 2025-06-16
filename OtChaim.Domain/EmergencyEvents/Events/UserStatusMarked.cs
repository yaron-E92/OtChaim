using System;
using OtChaim.Domain.Common;
using OtChaim.Domain.Users;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class UserStatusMarked : IEvent
    {
        public Guid EmergencySituationId { get; }
        public Guid UserId { get; }
        public UserStatus Status { get; }
        public string Message { get; }
        public DateTime OccurredOn { get; }

        public UserStatusMarked(
            Guid emergencySituationId,
            Guid userId,
            UserStatus status,
            string message = null)
        {
            EmergencySituationId = emergencySituationId;
            UserId = userId;
            Status = status;
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }
}