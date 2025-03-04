using FactoryTalent.Common.Application.Messaging;

namespace FactoryTalent.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
