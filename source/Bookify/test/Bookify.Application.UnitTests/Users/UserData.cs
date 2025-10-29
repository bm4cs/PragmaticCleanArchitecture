using Bookify.Domain.Users;

namespace Bookify.Application.UnitTests.Users;

internal class UserData
{
    public static User Create() => User.Create(FirstName, LastName, Email);

    public static readonly FirstName FirstName = new("John");
    public static readonly LastName LastName = new("Carmack");
    public static readonly Email Email = new("john@id.com");
}
