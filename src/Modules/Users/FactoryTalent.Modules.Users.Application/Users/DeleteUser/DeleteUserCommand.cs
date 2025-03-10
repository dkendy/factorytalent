
using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Modules.Users.Domain.Users;

namespace FactoryTalent.Modules.Users.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid id)
    : ICommand<Guid>;
