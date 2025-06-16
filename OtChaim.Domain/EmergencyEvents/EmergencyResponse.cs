using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents
{
    public class EmergencyResponse : ValueObject
    {
        public Guid UserId { get; private set; }
        public bool IsSafe { get; private set; }
        public string Message { get; private set; }
        public DateTime RespondedAt { get; private set; }

        private EmergencyResponse() { } // For EF Core

        public EmergencyResponse(Guid userId, bool isSafe, string message = null)
        {
            UserId = userId;
            IsSafe = isSafe;
            Message = message;
            RespondedAt = DateTime.UtcNow;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return IsSafe;
            yield return Message;
            yield return RespondedAt;
        }
    }
} 