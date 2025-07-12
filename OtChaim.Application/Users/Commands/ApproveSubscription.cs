using OtChaim.Application.Common;

namespace OtChaim.Application.Users.Commands;

/// <summary>
/// Command to approve a user subscription.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApproveSubscription"/> class.
/// </remarks>
/// <param name="subscriberId">The ID of the subscriber.</param>
/// <param name="subscribedToId">The ID of the user being subscribed to.</param>
public class ApproveSubscription(Guid subscriberId, Guid subscribedToId) : ICommand
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
