using System;
using System.Collections.Generic;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents;

public class EmergencyEvent : Entity
{
    public Guid InitiatorId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public EmergencyEventStatus Status { get; private set; }
    private readonly List<EmergencyResponse> _responses = new();
    public IReadOnlyList<EmergencyResponse> Responses => _responses.AsReadOnly();

    private EmergencyEvent() { } // For EF Core

    public EmergencyEvent(Guid initiatorId)
    {
        InitiatorId = initiatorId;
        CreatedAt = DateTime.UtcNow;
        Status = EmergencyEventStatus.Active;
    }

    public void AddResponse(Guid userId, bool isSafe, string? message = null)
    {
        var response = new EmergencyResponse(userId, isSafe, message);
        _responses.Add(response);

        // If all subscribers have responded, mark the event as resolved
        if (AreAllSubscribersResponded())
        {
            Resolve();
        }
    }

    public void Resolve()
    {
        if (Status == EmergencyEventStatus.Active)
        {
            Status = EmergencyEventStatus.Resolved;
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

public enum EmergencyEventStatus
{
    Active,
    Resolved
} 