using OtChaim.Domain.Common;
using OtChaim.Domain.Notifications;
using OtChaim.Domain.Users.Events;

namespace OtChaim.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
    private readonly List<Guid> _subscriberIds = new();
    public IReadOnlyList<Guid> SubscriberIds => _subscriberIds.AsReadOnly();
    private readonly List<NotificationChannel> _notificationChannels = new();
    public IReadOnlyList<NotificationChannel> NotificationChannels => _notificationChannels.AsReadOnly();
    private readonly List<Subscription> _subscriptions = new();
    public IReadOnlyList<Subscription> Subscriptions => _subscriptions.AsReadOnly();

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

    public void RaiseEvent(IEvent userEvent)
    {
        switch (userEvent)
        {
            case SubscriptionRequested subscriptionRequested:
                OnSubscriptionRequested(subscriptionRequested);
                break;
            // Add more cases for other event types as needed
            default:
                throw new NotImplementedException($"No handler for event type {userEvent.GetType().Name}");
        }
    }

    private bool RequiresSubscriptionApproval() => true; // Placeholder

    public void OnSubscriptionRequested(SubscriptionRequested subscriptionEvent)
    {
        var subscription = new Subscription(subscriptionEvent.SubscriberId, subscriptionEvent.SubscribedToId, RequiresSubscriptionApproval());

        _subscriptions.Add(subscription);
    }
}
