using Microsoft.AspNetCore.Routing;

namespace FactoryTalent.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
