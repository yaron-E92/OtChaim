using OtChaim.Application.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Application.EmergencyEvents.Commands;

public class StartEmergencySituation : ICommand
{
    public Guid InitiatorUserId { get; }
    public EmergencyType Type { get; }
    public Location Location { get; }
    public Area Area { get; }
    public string Description { get; }

    public StartEmergencySituation(
        Guid initiatorUserId,
        EmergencyType type,
        Location location,
        Area area,
        string description)
    {
        InitiatorUserId = initiatorUserId;
        Type = type;
        Location = location;
        Area = area;
        Description = description;
    }
}