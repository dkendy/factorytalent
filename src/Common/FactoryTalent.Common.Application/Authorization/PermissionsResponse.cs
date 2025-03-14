﻿namespace FactoryTalent.Common.Application.Authorization;

public sealed record PermissionsResponse(Guid UserId, HashSet<string> Permissions, string RoleName, int Level);
