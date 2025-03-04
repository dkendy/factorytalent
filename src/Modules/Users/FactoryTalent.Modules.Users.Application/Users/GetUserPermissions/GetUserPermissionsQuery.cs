using FactoryTalent.Common.Application.Authorization;
using FactoryTalent.Common.Application.Messaging;

namespace FactoryTalent.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
