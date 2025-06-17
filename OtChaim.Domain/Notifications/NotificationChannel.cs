using System.Collections.Generic;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Notifications;

public class NotificationChannel : ValueObject
{
    public string ChannelType { get; }
    public string? Address { get; }

    public NotificationChannel(string channelType, string? address = null)
    {
        ChannelType = channelType;
        Address = address;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChannelType;
        yield return Address;
    }

    public static NotificationChannel Sms(string phoneNumber) => new("SMS", phoneNumber);
    public static NotificationChannel Email(string email) => new("Email", email);
    public static NotificationChannel Push(string? deviceToken = null) => new("Push", deviceToken);
} 