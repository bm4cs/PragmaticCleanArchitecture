namespace Bookify.Domain.Abstractions;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        Id = id;
    }

    public Guid Id { get; init; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is null || obj.GetType() != GetType())
            return false;
        return Equals((Entity)obj);
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}
