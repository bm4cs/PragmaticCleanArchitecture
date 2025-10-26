using System;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bookify.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings =
        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

    private readonly IDateTimeProvider _dateTimeProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeProvider dateTimeProvider
    )
        : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await PublishDomainEventsAsOutboxMessagesAsync().ConfigureAwait(false);

            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    private async Task PublishDomainEventsAsOutboxMessagesAsync()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e =>
            {
                var domainEvents = e.GetDomainEvents();
                e.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)
            ))
            .ToList();

        await AddRangeAsync(outboxMessages);
    }
}
