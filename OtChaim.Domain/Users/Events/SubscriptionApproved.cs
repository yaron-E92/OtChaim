using Yaref92.Events;

namespace OtChaim.Domain.Users.Events;

/// <summary>
/// Event raised when a subscription is approved.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SubscriptionApproved"/> class.
/// </remarks>
/// <param name="subscriberId">The ID of the subscriber</param>
/// <param name="subscribedToId">The ID of the user being subscribed to</param>
public class SubscriptionApproved(Guid subscriberId, Guid subscribedToId, DateTime occurred = default,
                                  Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// The ID of the subscriber.
    /// </summary>
    public Guid SubscriberId { get; } = subscriberId;
    /// <summary>
    /// The ID of the user being subscribed to.
    /// </summary>
    public Guid SubscribedToId { get; } = subscribedToId;
}
