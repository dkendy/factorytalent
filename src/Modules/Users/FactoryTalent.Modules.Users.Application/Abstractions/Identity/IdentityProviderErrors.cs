using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Application.Abstractions.Identity;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique.");

    public static readonly Error SearchError = Error.Failure(
       "Identity.SearchError",
       "An error occurred while searching for a user.");
}
