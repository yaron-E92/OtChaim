using OtChaim.Application.Common;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Application.EmergencyEvents.Commands;

/// <summary>
/// Command to start a new emergency event.
/// </summary>
/// <param name="initiatorUserId">The ID of the user initiating the emergency.</param>
/// <param name="type">The type of emergency.</param>
/// <param name="location">The location of the emergency.</param>
/// <param name="affectedAreas">The affected areas</param>
/// <param name="severity">The severity of the emergency.</param>
/// <param name="description">A description of the emergency.</param>
public class StartEmergency(
    Guid initiatorUserId,
    EmergencyType type,
    Location location,
    IEnumerable<Area> affectedAreas,
    Severity severity,
    string description = "") : ICommand
{
    /// <summary>
    /// The ID of the user initiating the emergency.
    /// </summary>
    public Guid InitiatorUserId { get; } = initiatorUserId;
    /// <summary>
    /// The type of emergency.
    /// </summary>
    public EmergencyType Type { get; } = type;
    /// <summary>
    /// The location of the emergency.
    /// </summary>
    public Location Location { get; } = location;
    /// <summary>
    /// The affected area.
    /// </summary>
    public IEnumerable<Area> AffectedAreas { get; } = affectedAreas;
    /// <summary>
    /// The severity of the emergency.
    /// </summary>
    public Severity Severity { get; } = severity;
    /// <summary>
    /// The description of the emergency.
    /// </summary>
    public string Description { get; } = description;
}
