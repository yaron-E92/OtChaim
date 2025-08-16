using OtChaim.Domain.Common;
using OtChaim.Domain.Notifications;
using OtChaim.Domain.Users.Events;

namespace OtChaim.Domain.Users;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : Entity
{
    /// <summary>
    /// Gets the user's name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public string Email { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    public string PhoneNumber { get; private set; } = string.Empty;
    /// <summary>
    /// Gets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; private set; }
    private readonly List<Guid> _subscriberIds = [];
    /// <summary>
    /// Gets the list of subscriber IDs.
    /// </summary>
    public IReadOnlyList<Guid> SubscriberIds => _subscriberIds.AsReadOnly();
    private readonly List<NotificationChannel> _notificationChannels = [];
    /// <summary>
    /// Gets the notification channels for the user.
    /// </summary>
    public IReadOnlyList<NotificationChannel> NotificationChannels => _notificationChannels.AsReadOnly();
    private readonly List<Subscription> _subscriptions = [];
    private bool _requireApproval = true;

    /// <summary>
    /// Gets the subscriptions for the user.
    /// </summary>
    public IReadOnlyList<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    /// <summary>
    /// Gets a user instance representing no user.
    /// </summary>
    public static User None { get; } = new User { Id = Guid.Empty };

    private User() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    public User(string name, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        IsActive = true;
    }

    /// <summary>
    /// Adds a subscriber to the user.
    /// </summary>
    public void AddSubscriber(Guid subscriberId)
    {
        if (!_subscriberIds.Contains(subscriberId))
        {
            _subscriberIds.Add(subscriberId);
        }
    }

    /// <summary>
    /// Removes a subscriber from the user.
    /// </summary>
    public void RemoveSubscriber(Guid subscriberId)
    {
        _subscriberIds.Remove(subscriberId);
    }

    /// <summary>
    /// Deactivates the user.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Activates the user.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Toggles the subscription approval requirement.
    /// </summary>
    public void ToggleApproval()
    {
        _requireApproval = !_requireApproval;
    }

    /// <summary>
    /// Returns whether the user requires subscription approval.
    /// </summary>
    public bool RequiresSubscriptionApproval() => _requireApproval;

    /// <summary>
    /// Handles a subscription requested event.
    /// </summary>
    public void OnSubscriptionRequested(SubscriptionRequested subscriptionEvent)
    {
        Subscription? subscription = new Subscription(subscriptionEvent.SubscriberId, subscriptionEvent.SubscribedToId, RequiresSubscriptionApproval());
        _subscriptions.Add(subscription);
    }

    /// <summary>
    /// Handles a subscription approved event.
    /// </summary>
    public void OnSubscriptionApproved(SubscriptionApproved subscriptionEvent)
    {
        Subscription? subscription = _subscriptions.FirstOrDefault(s => s.SubscriberId == subscriptionEvent.SubscriberId && s.SubscribedToId == subscriptionEvent.SubscribedToId);
        subscription?.Approve();
    }

    /// <summary>
    /// Handles a subscription rejected event.
    /// </summary>
    public void OnSubscriptionRejected(SubscriptionRejected subscriptionEvent)
    {
        Subscription? subscription = _subscriptions.FirstOrDefault(s => s.SubscriberId == subscriptionEvent.SubscriberId && s.SubscribedToId == subscriptionEvent.SubscribedToId);
        subscription?.Reject();
    }
}
