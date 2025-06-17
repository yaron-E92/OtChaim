using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class EmergencySituationStarted : IEvent
    {
        public Guid EmergencySituationId { get; }
        public Guid InitiatorId { get; }
        public EmergencyType EmergencyType { get; }
        public Location Location { get; }
        public Area AffectedArea { get; }
        public DateTime StartedOn => OccurredOn;
        public DateTime OccurredOn { get; }

        public EmergencySituationStarted(
            Guid emergencySituationId,
            Guid initiatorId,
            EmergencyType emergencyType,
            Location location,
            Area affectedArea)
        {
            EmergencySituationId = emergencySituationId;
            InitiatorId = initiatorId;
            EmergencyType = emergencyType;
            Location = location;
            AffectedArea = affectedArea;
            OccurredOn = DateTime.UtcNow;
        }
    }
}