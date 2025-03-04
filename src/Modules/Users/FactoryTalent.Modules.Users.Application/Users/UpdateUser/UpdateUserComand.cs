using FactoryTalent.Common.Application.Messaging;

namespace FactoryTalent.Modules.Users.Application.Users.UpdateUser;
 
public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName, string CPF, string Address) : ICommand;
