using OtChaim.Application.Common;

namespace OtChaim.Application.EmergencyEvents.Commands;

public class EndEmergency(Guid emergencyId, string? resolutionNote = null) : ICommand
{
    public Guid EmergencyId { get; } = emergencyId;
    public string? ResolutionNote { get; } = resolutionNote;
} 
