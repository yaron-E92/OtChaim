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
    public ChannelType ChannelType { get; private set; }
    /// <summary>
    /// Gets the address associated with the channel, if any.
    /// </summary>
    public string Address { get; private set; } = string.Empty;

    private NotificationChannel() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationChannel"/> class.
    /// </summary>
    public NotificationChannel(ChannelType channelType, string address)
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
    public static NotificationChannel Sms(string phoneNumber) => new(ChannelType.Sms, phoneNumber);
    /// <summary>
    /// Creates an email notification channel.
    /// </summary>
    public static NotificationChannel Email(string email) => new(ChannelType.Email, email);
    /// <summary>
    /// Creates a push notification channel.
    /// </summary>
    public static NotificationChannel Push(string deviceToken = "") => new(ChannelType.Push, deviceToken);
}
