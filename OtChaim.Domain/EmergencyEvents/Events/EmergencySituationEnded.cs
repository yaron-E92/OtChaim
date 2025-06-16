using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class EmergencySituationEnded : IEvent
    {
        public Guid EmergencySituationId { get; }
        public DateTime EndedOn { get; }

        public EmergencySituationEnded(Guid emergencySituationId)
        {
            EmergencySituationId = emergencySituationId;
            EndedOn = DateTime.UtcNow;
        }
    }
}