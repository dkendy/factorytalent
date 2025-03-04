using FactoryTalent.Common.Application.Messaging;

namespace FactoryTalent.Modules.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName, string CPF, DateTime birthdate, Guid? superiorId, string Address, List<string> contatos)
    : ICommand<Guid>;
