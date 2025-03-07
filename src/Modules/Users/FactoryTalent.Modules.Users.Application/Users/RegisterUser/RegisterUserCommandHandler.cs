using System.Globalization;
using System.Security.Claims;
using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using FactoryTalent.Modules.Users.Application.Abstractions.Identity;
using FactoryTalent.Modules.Users.Domain.Users;
using Microsoft.AspNetCore.Http;
using Polly;
using Serilog;

namespace FactoryTalent.Modules.Users.Application.Users.RegisterUser;

 
internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

        int? level = _user?.Claims.FirstOrDefault(x => x.Type == "level")?.Value is string levelValue && int.TryParse(levelValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedLevel) ? parsedLevel : (int?)null;

        if (level < request.Role.Level)
        {
            return Result.Failure<Guid>(UserErrors.ArgumentLevelError(request.Role.Name));
        }

        User? @userDoc = await userRepository.GetByCPFAsync(request.CPF, cancellationToken);

        if (@userDoc is not null)
        {
            return Result.Failure<Guid>(UserErrors.ArgumentErrorCPF(request.CPF));
        }

        @userDoc = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (@userDoc is not null)
        {
            return Result.Failure<Guid>(UserErrors.ArgumentErrorEmail(request.Email));
        }

        string keyCloakGuid = "";

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
            Result<string> result = await identityProviderService.RegisterUserAsync(
                            new UserModel(request.Email, request.Password, "Adm", "Adm"),
                            new CancellationToken());

            if (result.IsFailure)
            {
                Result<string> resultEmail = await identityProviderService.GetUserAsync(request.Email,
                   cancellationToken);
                if (!resultEmail.IsFailure)
                {
                    keyCloakGuid = resultEmail.Value;
                }

                return resultEmail;
            }
            else
            {
                keyCloakGuid = result.Value;
            }

            return result;
        });

        var user = User.Create(request.Email, request.FirstName, request.LastName, request.CPF, request.Address, DateTime.Now, request.superiorId, keyCloakGuid, request.contatos, request.Role);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
