using Bookify.Domain.Users;

namespace Bookify.Domain.UnitTests.Users;

public class UserData
{
    public static readonly FirstName FirstName = new("John");
    public static readonly LastName LastName = new("Carmack");
    public static readonly Email Email = new("john@id.com");
}
