using OtChaim.Domain.Common;
using Yaref92.Events.Abstractions;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class EmergencySituationStarted : IDomainEvent
    {
        public Guid EmergencySituationId { get; }
        public Guid InitiatorId { get; }
        public EmergencyType EmergencyType { get; }
        public Location Location { get; }
        public Area AffectedArea { get; }
        public DateTime StartedOn => DateTimeOccurredUtc;
        public DateTime DateTimeOccurredUtc { get; }

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
            DateTimeOccurredUtc = DateTime.UtcNow;
        }
    }
}
