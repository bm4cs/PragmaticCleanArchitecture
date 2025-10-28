using Bookify.Infrastructure.Clock;
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
            "Host=172.21.32.110;Port=5432;Database=bookify;Username=postgres;Password=postgres;Include Error Detail=true";
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new ApplicationDbContext(optionsBuilder.Options, new DateTimeProvider());
    }
}
