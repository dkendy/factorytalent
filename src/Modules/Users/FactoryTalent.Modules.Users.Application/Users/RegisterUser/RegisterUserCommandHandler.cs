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
        Result<string> result = await identityProviderService.RegisterUserAsync(
            new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        var user = User.Create(request.Email, request.FirstName, request.LastName, request.CPF, request.Address, DateTime.Now, request.superiorId, result.Value, request.contatos);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
