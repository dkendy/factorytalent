using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId)
    {
        return Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");
    }


    public static Error NotFound(string identityId)
    {
        return Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");
    }


    public static Error ArgumentError(DateTime datetime)
    {
        return Error.NotFound("Users.ArgumentError", $"The user birthday {datetime} must be upper than 18 years old");
    }


    public static Error ArgumentErrorCPF(string CPF)
    {
        return Error.NotFound("Users.ArgumentError", $"A record with this document already exists - {CPF}");
    }

    public static Error ArgumentErrorEmail(string email)
    {
        return Error.NotFound("Users.ArgumentError", $"A record with this e-mail already exists - {email}");
    }
}
