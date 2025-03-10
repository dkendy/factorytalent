using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);

    Task<Result> DeleteUserAsync(Guid identityId, CancellationToken cancellationToken = default);

    Task<Result<string>> GetUserAsync(string email, CancellationToken cancellationToken = default);
}
