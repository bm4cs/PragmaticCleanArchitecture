using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bookify.Infrastructure;

public class ApplicationDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use a connection string for design-time
        var connectionString =
            "Host=localhost;Port=5432;Database=bookify;Username=postgres;Password=postgres;Include Error Detail=true";
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();

        // Create a dummy publisher for design-time
        var dummyPublisher = new DummyPublisher();

        return new ApplicationDbContext(optionsBuilder.Options, dummyPublisher);
    }

    private class DummyPublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default
        )
            where TNotification : INotification => Task.CompletedTask;
    }
}
