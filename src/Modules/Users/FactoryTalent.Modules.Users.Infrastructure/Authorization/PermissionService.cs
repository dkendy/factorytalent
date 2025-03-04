﻿using FactoryTalent.Common.Application.Authorization;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace FactoryTalent.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
