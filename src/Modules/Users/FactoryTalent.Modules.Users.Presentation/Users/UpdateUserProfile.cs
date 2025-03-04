using System.Security.Claims;
using FactoryTalent.Common.Domain;
using FactoryTalent.Common.Infrastructure.Authentication;
using FactoryTalent.Common.Presentation.Endpoints;
using FactoryTalent.Common.Presentation.Results;
using FactoryTalent.Modules.Users.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FactoryTalent.Modules.Users.Presentation.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile", async (Request request,  ClaimsPrincipal claims, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateUserCommand(
                claims.GetUserId(),
                request.FirstName,
                request.LastName,
                request.CPF,
                request.Address));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyUser)
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Address { get; init; }

        public string CPF { get; init; }
    }
}
