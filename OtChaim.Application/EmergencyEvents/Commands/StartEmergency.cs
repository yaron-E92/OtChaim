using OtChaim.Application.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Application.EmergencyEvents.Commands;

public class StartEmergency(
    Guid initiatorUserId,
    EmergencyType type,
    Location location,
    Area area,
    Severity severity,
    string description = "") : ICommand
{
    public Guid InitiatorUserId { get; } = initiatorUserId;
    public EmergencyType Type { get; } = type;
    public Location Location { get; } = location;
    public Area Area { get; } = area;
    public Severity Severity { get; } = severity;
    public string Description { get; } = description;
}
