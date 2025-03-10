using System.Net;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace FactoryTalent.Modules.Users.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    // POST /admin/realms/{realm}/users
    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User registration failed");

            return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
        }
    }

    // POST /admin/realms/{realm}/users
    public async Task<Result<string>> GetUserAsync(string email, CancellationToken cancellationToken = default)
    {
        
        try
        {
            string identityId = await keyCloakClient.GetUserByEmailAsync(email, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(exception, "User search failed");

            return Result.Failure<string>(IdentityProviderErrors.SearchError);
        }
    }

    // DELETE /admin/realms/{realm}/users/{id} 

    public async Task<Result> DeleteUserAsync(Guid identityId, CancellationToken cancellationToken = default)
    {
        try
        {
            await keyCloakClient.DeleteUserAsync(identityId, cancellationToken);
            return Result.Success();
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogError(exception, "User deletion failed");
            return Result.Failure(IdentityProviderErrors.UserNotFound);
        }
    }


}
