using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");

    public static Error NotFound(string identityId) =>
        Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");

    public static Error ArgumentError(DateTime datetime) =>
      Error.NotFound("Users.ArgumentError", $"The user birthday {datetime} must be upper than 18 years old");

    public static Error ArgumentError(string CPF) =>
        Error.NotFound("Users.ArgumentError", $"A record with this document already exists - {CPF}");

}
