using System;
using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{

}
