using System;

namespace OrderService.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
