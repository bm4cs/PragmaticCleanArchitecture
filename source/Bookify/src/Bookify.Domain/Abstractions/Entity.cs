namespace Bookify.Domain.Abstractions;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; init; } = id;

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

    public IReadOnlyList<IDomainEvent> GetDomainEvents => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}
