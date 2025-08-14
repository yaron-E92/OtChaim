namespace OtChaim.Domain.Common;

/// <summary>
/// Represents a base entity with an identity.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current entity.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return GetType() == other.GetType()
            && Id == other.Id;
    }

    /// <summary>
    /// Returns a hash code for the entity.
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
