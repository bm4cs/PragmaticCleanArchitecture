﻿#pragma warning disable CA1711
namespace Bookify.Domain.Users;

public sealed class Permission
{
    public static readonly Permission UsersRead = new(1, "users:read");

    private Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}
