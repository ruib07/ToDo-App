using ToDo.Server.Helpers;
using ToDo.Server.Models;

namespace ToDo.Tests.Templates;

public static class UsersTests
{
    public static Users CreateUser()
    {
        return new Users()
        {
            Id = Guid.NewGuid(),
            Name = "User Test",
            Email = "usertest@email.com",
            Password = PasswordHasherHelper.HashPassword("User@Test-123")
        };
    }

    public static Users UpdateUser(Guid id)
    {
        return new Users()
        {
            Id = id,
            Name = "User Updated",
            Email = "userupdated@email.com",
            Password = PasswordHasherHelper.HashPassword("User@Updated-123")
        };
    }
}
