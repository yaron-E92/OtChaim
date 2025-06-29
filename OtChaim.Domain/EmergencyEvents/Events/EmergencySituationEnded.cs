using Yaref92.Events.Abstractions;

namespace OtChaim.Domain.EmergencyEvents.Events
{
    public class EmergencySituationEnded : IDomainEvent
    {
        public Guid EmergencySituationId { get; }
        public DateTime EndedOn => DateTimeOccurredUtc;
        public DateTime DateTimeOccurredUtc { get; }

        public EmergencySituationEnded(Guid emergencySituationId)
        {
            EmergencySituationId = emergencySituationId;
            DateTimeOccurredUtc = DateTime.UtcNow;
        }
    }
}
