using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using FactoryTalent.Modules.Users.Application.Abstractions.Identity;
using FactoryTalent.Modules.Users.Domain.Users;

namespace FactoryTalent.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProviderService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        User? @userDoc = await userRepository.GetByCPFAsync(request.CPF, cancellationToken);

        if (@userDoc is null)
        {
            return Result.Failure<Guid>(UserErrors.ArgumentErrorCPF(request.CPF));
        }

        @userDoc = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (@userDoc is null)
        {
            return Result.Failure<Guid>(UserErrors.ArgumentErrorEmail(request.CPF));
        }


        Result<string> result = await identityProviderService.RegisterUserAsync(
            new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);
        
        string keyCloakGuid = ""; 

        if (result.IsFailure)
        {
            Result<string> resultEmail = await identityProviderService.GetUserAsync(request.Email,
               cancellationToken);
            if (resultEmail.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            else
            {
                keyCloakGuid = resultEmail.Value;

            } 
        }
        else
        {
            keyCloakGuid = result.Value;
        }

        

        var user = User.Create(request.Email, request.FirstName, request.LastName, request.CPF, request.Address, DateTime.Now, request.superiorId, keyCloakGuid, request.contatos);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
