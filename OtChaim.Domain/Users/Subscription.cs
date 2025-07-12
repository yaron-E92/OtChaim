using OtChaim.Domain.Common;

namespace OtChaim.Domain.Users;

public enum SubscriptionStatus { Pending, Approved, Rejected }

/// <summary>
/// Represents a subscription between two users.
/// </summary>
public class Subscription : Entity
{
    /// <summary>
    /// Gets the ID of the subscriber.
    /// </summary>
    public Guid SubscriberId { get; private set; }
    /// <summary>
    /// Gets the ID of the user being subscribed to.
    /// </summary>
    public Guid SubscribedToId { get; private set; }
    /// <summary>
    /// Gets the status of the subscription.
    /// </summary>
    public SubscriptionStatus Status { get; private set; }
    /// <summary>
    /// Gets the creation time of the subscription.
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    /// <summary>
    /// Gets the time the subscription was approved, if any.
    /// </summary>
    public DateTime? ApprovedAt { get; private set; }
    /// <summary>
    /// Gets the last known status of the user.
    /// </summary>
    public UserStatus? LastKnownStatus { get; private set; }

    private Subscription() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subscription"/> class.
    /// </summary>
    /// <param name="subscriberId">The ID of the subscriber</param>
    /// <param name="subscribedToId">The ID of the user being subscribed to</param>
    /// <param name="requiresApproval">Whether the subcription requires approval by the recipient</param>
    public Subscription(Guid subscriberId, Guid subscribedToId, bool requiresApproval)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
        CreatedAt = DateTime.UtcNow;
        Status = requiresApproval ? SubscriptionStatus.Pending : SubscriptionStatus.Approved;
    }

    /// <summary>
    /// Approves the subscription.
    /// </summary>
    public void Approve()
    {
        Status = SubscriptionStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Rejects the subscription.
    /// </summary>
    public void Reject()
    {
        Status = SubscriptionStatus.Rejected;
    }

    /// <summary>
    /// Updates the last known status of the user.
    /// </summary>
    public void UpdateLastKnownStatus(UserStatus status)
    {
        LastKnownStatus = status;
    }
}
