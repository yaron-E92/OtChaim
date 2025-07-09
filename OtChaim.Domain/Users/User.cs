using OtChaim.Domain.Common;
using OtChaim.Domain.Notifications;
using OtChaim.Domain.Users.Events;

namespace OtChaim.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    private readonly List<Guid> _subscriberIds = [];
    public IReadOnlyList<Guid> SubscriberIds => _subscriberIds.AsReadOnly();
    private readonly List<NotificationChannel> _notificationChannels = [];
    public IReadOnlyList<NotificationChannel> NotificationChannels => _notificationChannels.AsReadOnly();
    private readonly List<Subscription> _subscriptions = [];
    public IReadOnlyList<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    public static User None { get; } = new User{ Id = Guid.Empty };

    private User() { } // For EF Core

    public User(string name, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        IsActive = true;
    }

    public void AddSubscriber(Guid subscriberId)
    {
        if (!_subscriberIds.Contains(subscriberId))
        {
            _subscriberIds.Add(subscriberId);
        }
    }

    public void RemoveSubscriber(Guid subscriberId)
    {
        _subscriberIds.Remove(subscriberId);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public bool RequiresSubscriptionApproval() => true; // Placeholder

    public void OnSubscriptionRequested(SubscriptionRequested subscriptionEvent)
    {
        Subscription? subscription = new Subscription(subscriptionEvent.SubscriberId, subscriptionEvent.SubscribedToId, RequiresSubscriptionApproval());
        _subscriptions.Add(subscription);
    }

    public void OnSubscriptionApproved(SubscriptionApproved subscriptionEvent)
    {
        Subscription? subscription = _subscriptions.FirstOrDefault(s => s.SubscriberId == subscriptionEvent.SubscriberId && s.SubscribedToId == subscriptionEvent.SubscribedToId);
        subscription?.Approve();
    }

    public void OnSubscriptionRejected(SubscriptionRejected subscriptionEvent)
    {
        Subscription? subscription = _subscriptions.FirstOrDefault(s => s.SubscriberId == subscriptionEvent.SubscriberId && s.SubscribedToId == subscriptionEvent.SubscribedToId);
        subscription?.Reject();
    }
}
