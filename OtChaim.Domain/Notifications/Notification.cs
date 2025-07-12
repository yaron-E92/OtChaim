using OtChaim.Domain.Common;

namespace OtChaim.Domain.Notifications;

/// <summary>
/// Represents a notification sent to a user.
/// </summary>
public class Notification : Entity
{
    /// <summary>
    /// Gets the ID of the user to whom the notification was sent.
    /// </summary>
    public Guid UserId { get; private set; }
    /// <summary>
    /// Gets the ID of the related emergency.
    /// </summary>
    public Guid EmergencyId { get; private set; }
    /// <summary>
    /// Gets the type of the notification.
    /// </summary>
    public NotificationType Type { get; private set; }
    /// <summary>
    /// Gets the message of the notification.
    /// </summary>
    public string Message { get; private set; }
    /// <summary>
    /// Gets the creation time of the notification.
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// Gets the time the notification was read, if any.
    /// </summary>
    public DateTime? ReadAt { get; private set; }
    /// <summary>
    /// Gets the status of the notification.
    /// </summary>
    public NotificationStatus Status { get; private set; }

    private Notification() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    public Notification(Guid userId, Guid emergencyId, NotificationType type, string message)
    {
        UserId = userId;
        EmergencyId = emergencyId;
        Type = type;
        Message = message;
        CreatedAt = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }

    /// <summary>
    /// Marks the notification as read.
    /// </summary>
    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Pending)
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Marks the notification as failed.
    /// </summary>
    public void MarkAsFailed()
    {
        if (Status == NotificationStatus.Pending)
        {
            Status = NotificationStatus.Failed;
        }
    }
}

/// <summary>
/// The type of notification.
/// </summary>
public enum NotificationType
{
    EmergencyAlert,
    ResponseReceived,
    EventResolved
}

/// <summary>
/// The status of a notification.
/// </summary>
public enum NotificationStatus
{
    Pending,
    Read,
    Failed
}
