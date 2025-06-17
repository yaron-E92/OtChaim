using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Notifications;

public class Notification : Entity
{
    public Guid UserId { get; private set; }
    public Guid EmergencyEventId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public NotificationStatus Status { get; private set; }

    private Notification() { } // For EF Core

    public Notification(Guid userId, Guid emergencyEventId, NotificationType type, string message)
    {
        UserId = userId;
        EmergencyEventId = emergencyEventId;
        Type = type;
        Message = message;
        CreatedAt = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Pending)
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }

    public void MarkAsFailed()
    {
        if (Status == NotificationStatus.Pending)
        {
            Status = NotificationStatus.Failed;
        }
    }
}

public enum NotificationType
{
    EmergencyAlert,
    ResponseReceived,
    EventResolved
}

public enum NotificationStatus
{
    Pending,
    Read,
    Failed
} 