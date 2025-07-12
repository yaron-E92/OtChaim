using OtChaim.Domain.Common;

namespace OtChaim.Domain.Notifications;

/// <summary>
/// Represents a notification channel for a user.
/// </summary>
public class NotificationChannel : ValueObject
{
    /// <summary>
    /// Gets the type of the notification channel.
    /// </summary>
    public string ChannelType { get; private set; }
    /// <summary>
    /// Gets the address associated with the channel, if any.
    /// </summary>
    public string? Address { get; private set; }

    private NotificationChannel() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationChannel"/> class.
    /// </summary>
    public NotificationChannel(string channelType, string? address = null)
    {
        ChannelType = channelType;
        Address = address;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChannelType;
        yield return Address;
    }

    /// <summary>
    /// Creates an SMS notification channel.
    /// </summary>
    public static NotificationChannel Sms(string phoneNumber) => new("SMS", phoneNumber);
    /// <summary>
    /// Creates an email notification channel.
    /// </summary>
    public static NotificationChannel Email(string email) => new("Email", email);
    /// <summary>
    /// Creates a push notification channel.
    /// </summary>
    public static NotificationChannel Push(string? deviceToken = null) => new("Push", deviceToken);
}
