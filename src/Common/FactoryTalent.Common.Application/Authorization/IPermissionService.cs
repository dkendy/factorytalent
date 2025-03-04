using FactoryTalent.Common.Domain;

namespace FactoryTalent.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
