using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Represents an emergency event in the system.
/// </summary>
public class Emergency : Entity
{
    /// <summary>
    /// Gets the location of the emergency.
    /// </summary>
    public Location Location { get; private set; }
    /// <summary>
    /// Gets the affected areas of the emergency.
    /// </summary>
    public IReadOnlyList<Area> AffectedAreas => _affectedAreas.AsReadOnly();
    private readonly List<Area> _affectedAreas = new();
    /// <summary>
    /// Gets the severity of the emergency.
    /// </summary>
    public Severity Severity { get; private set; }
    /// <summary>
    /// Gets the creation time of the emergency.
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// Gets the time the emergency was resolved, if any.
    /// </summary>
    public DateTime? ResolvedAt { get; private set; }
    /// <summary>
    /// Gets the status of the emergency.
    /// </summary>
    public EmergencyStatus Status { get; private set; }
    private readonly List<EmergencyResponse> _responses = new();
    /// <summary>
    /// Gets the responses to the emergency.
    /// </summary>
    public IReadOnlyList<EmergencyResponse> Responses => _responses.AsReadOnly();
    /// <summary>
    /// Gets the type of the emergency.
    /// </summary>
    public EmergencyType? EmergencyType { get; private set; }

    private Emergency() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="Emergency"/> class.
    /// </summary>
    public Emergency(Location location, IEnumerable<Area>? affectedAreas = null, Severity severity = Severity.Medium, EmergencyType? emergencyType = null)
    {
        ArgumentNullException.ThrowIfNull(location);

        Location = location;
        EmergencyType = emergencyType;
        _affectedAreas = (affectedAreas == null || !affectedAreas.Any())
            ? [Area.FromLocation(location.Clone(), emergencyType: emergencyType)]
            : new List<Area>(affectedAreas);
        Severity = severity;
        CreatedAt = DateTime.UtcNow;
        Status = EmergencyStatus.Active;
    }

    /// <summary>
    /// Adds a response to the emergency.
    /// </summary>
    public void AddResponse(Guid userId, bool isSafe, string message = "")
    {
        var response = new EmergencyResponse(userId, isSafe, message ?? "");
        _responses.Add(response);

        // If all subscribers have responded, mark the event as resolved
        if (AreAllSubscribersResponded())
        {
            Resolve();
        }
    }

    /// <summary>
    /// Resolves the emergency.
    /// </summary>
    public void Resolve()
    {
        if (Status == EmergencyStatus.Active)
        {
            Status = EmergencyStatus.Resolved;
            ResolvedAt = DateTime.UtcNow;
        }
    }

    private bool AreAllSubscribersResponded()
    {
        // This would need to be implemented based on your business logic
        // You might want to compare against a list of expected responders
        return false;
    }
}
