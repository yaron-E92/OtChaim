using Yaref92.Events;

namespace OtChaim.Domain.Users.Events;

/// <summary>
/// Event raised when a subscription is requested.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SubscriptionRequested"/> class.
/// </remarks>
/// <param name="subscriberId">The ID of the subscriber</param>
/// <param name="subscribedToId">The ID of the user being subscribed to</param>
public class SubscriptionRequested(Guid subscriberId, Guid subscribedToId, DateTime occurred = default,
                                   Guid eventId = default) : DomainEventBase(occurred, eventId)
{
    /// <summary>
    /// Gets the ID of the subscriber.
    /// </summary>
    public Guid SubscriberId { get; } = subscriberId;
    /// <summary>
    /// Gets the ID of the user being subscribed to.
    /// </summary>
    public Guid SubscribedToId { get; } = subscribedToId;
}
