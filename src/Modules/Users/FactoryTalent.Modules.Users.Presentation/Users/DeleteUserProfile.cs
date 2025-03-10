using System.Security.Claims;
using FactoryTalent.Common.Domain;
using FactoryTalent.Common.Presentation.Endpoints;
using FactoryTalent.Common.Presentation.Results;
using FactoryTalent.Modules.Users.Application.Users.DeleteUser;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using FactoryTalent.Modules.Users.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace FactoryTalent.Modules.Users.Presentation.Users;

internal sealed class DeleteUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("users/profile/{id}", async (Guid id, ClaimsPrincipal claims, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteUserCommand(id));
            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.DeleteUser)
        .WithTags(Tags.Users);
    }
     
}
