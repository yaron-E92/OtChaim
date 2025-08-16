namespace OtChaim.Domain.Common;

/// <summary>
/// Represents a base class for value objects.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the atomic values that define the value object.
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Returns a hash code for the value object.
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(1, (current, obj) =>
        {
            unchecked
            {
                return current * 23 + (obj?.GetHashCode() ?? 0);
            }
        });
    }
}
