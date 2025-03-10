using System.Globalization;
using System.Security.Claims;
using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using FactoryTalent.Modules.Users.Application.Abstractions.Identity;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.AspNetCore.Http;
using Polly;
using Serilog;

namespace FactoryTalent.Modules.Users.Application.Users.DeleteUser;
public class DeleteUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? _user = httpContextAccessor?.HttpContext?.User;

        if (_user?.Claims.FirstOrDefault()?.Value is string userClaimValue)
        {
            Log.Debug(userClaimValue);
        }
        else
        {
            Log.Debug("No user claim available");
        }

        User user = await userRepository.GetAsync(request.id, cancellationToken);

        if (!string.IsNullOrEmpty(user?.IdentityId) && Guid.TryParse(user.IdentityId, out Guid id))
        {
            Polly.Retry.AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryAttempt))),
                    (exception, timeSpan) =>
                    {
                        Console.WriteLine($"Keycloak not available, retrying in {timeSpan.TotalSeconds} seconds...");
                    });

            await retryPolicy.ExecuteAsync(async () =>
            {
                await identityProviderService.DeleteUserAsync(id, new CancellationToken());
            });
        }

        await userRepository.DeleteAsync(request.id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return request.id;
    }
}
