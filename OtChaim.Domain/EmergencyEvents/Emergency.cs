using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents;

public class Emergency : Entity
{
    public Location Location { get; private set; }
    public IReadOnlyList<Area> AffectedAreas => _affectedAreas.AsReadOnly();
    private readonly List<Area> _affectedAreas = new();
    public Severity Severity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public EmergencyStatus Status { get; private set; }
    private readonly List<EmergencyResponse> _responses = new();
    public IReadOnlyList<EmergencyResponse> Responses => _responses.AsReadOnly();
    public EmergencyType? EmergencyType { get; private set; }

    private Emergency() { } // For EF Core

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
