using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using FactoryTalent.Modules.Users.Domain.Users;

namespace FactoryTalent.Modules.Users.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.UserId));
        }

        user.Update(request.FirstName, request.LastName, request.Address);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
