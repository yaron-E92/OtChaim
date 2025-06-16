using OtChaim.Application.Common;
using OtChaim.Domain.Common;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Application.EmergencyEvents.Commands
{
    public class StartEmergencySituation : ICommand
    {
        public Guid InitiatorId { get; }
        public EmergencyType EmergencyType { get; }
        public Location Location { get; }
        public Area AffectedArea { get; }

        public StartEmergencySituation(
            Guid initiatorId,
            EmergencyType emergencyType,
            Location location,
            Area affectedArea)
        {
            InitiatorId = initiatorId;
            EmergencyType = emergencyType;
            Location = location;
            AffectedArea = affectedArea;
        }
    }
}