using System;
using OtChaim.Domain.Common;
using OtChaim.Domain.Users;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class SubscriberNotified : IEvent
    {
        public Guid EmergencySituationId { get; }
        public Guid SubscriberId { get; }
        public Guid UserId { get; }
        public UserStatus Status { get; }
        public string Message { get; }
        public DateTime OccurredOn { get; }

        public SubscriberNotified(
            Guid emergencySituationId,
            Guid subscriberId,
            Guid userId,
            UserStatus status,
            string message = null)
        {
            EmergencySituationId = emergencySituationId;
            SubscriberId = subscriberId;
            UserId = userId;
            Status = status;
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }
}