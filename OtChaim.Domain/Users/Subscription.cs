using System;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Users;

public enum SubscriptionStatus { Pending, Approved, Rejected }

public class Subscription : Entity
{
    public Guid SubscriberId { get; private set; }
    public Guid SubscribedToId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public UserStatus? LastKnownStatus { get; private set; }

    private Subscription() { }

    public Subscription(Guid subscriberId, Guid subscribedToId, bool requiresApproval)
    {
        SubscriberId = subscriberId;
        SubscribedToId = subscribedToId;
        CreatedAt = DateTime.UtcNow;
        Status = SubscriptionStatus.Pending;
        if (!requiresApproval)
        {
            Status = SubscriptionStatus.Approved;
        }
    }

    public void Approve()
    {
        Status = SubscriptionStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = SubscriptionStatus.Rejected;
    }

    public void UpdateLastKnownStatus(UserStatus status)
    {
        LastKnownStatus = status;
    }
} 