using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Represents a response to an emergency event.
/// </summary>
public class EmergencyResponse : ValueObject
{
    /// <summary>
    /// Gets the ID of the user who responded.
    /// </summary>
    public Guid UserId { get; private set; }
    /// <summary>
    /// Gets a value indicating whether the user is safe.
    /// </summary>
    public bool IsSafe { get; private set; }
    /// <summary>
    /// Gets the message provided by the user.
    /// </summary>
    public string Message { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the time the response was made.
    /// </summary>
    public DateTime RespondedAt { get; private set; }

    private EmergencyResponse() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyResponse"/> class.
    /// </summary>
    /// <param name="userId">Id of the responding user</param>
    /// <param name="isSafe">Is the user safe</param>
    /// <param name="message">Optional message from user</param>
    public EmergencyResponse(Guid userId, bool isSafe, string message = "")
    {
        UserId = userId;
        IsSafe = isSafe;
        Message = message;
        RespondedAt = DateTime.UtcNow;
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return IsSafe;
        yield return Message;
        yield return RespondedAt;
    }
}
