using OtChaim.Application.Common;

namespace OtChaim.Application.EmergencyEvents.Commands;

/// <summary>
/// Command to end an emergency event.
/// </summary>
/// <param name="emergencyId">The ID of the emergency to end.</param>
/// <param name="resolutionNote">An optional note describing the resolution.</param>
public class EndEmergency(Guid emergencyId, string? resolutionNote = null) : ICommand
{
    /// <summary>
    /// The ID of the emergency to end.
    /// </summary>
    public Guid EmergencyId { get; } = emergencyId;
    /// <summary>
    /// An optional note describing the resolution.
    /// </summary>
    public string? ResolutionNote { get; } = resolutionNote;
}
