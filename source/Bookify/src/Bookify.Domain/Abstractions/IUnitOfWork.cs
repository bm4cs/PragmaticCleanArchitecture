using Bookify.Domain.Users;

namespace Bookify.Domain.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(User user);
}
