using FactoryTalent.Common.Domain;
using FactoryTalent.Common.Presentation.Endpoints;
using FactoryTalent.Common.Presentation.Results;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FactoryTalent.Modules.Users.Presentation.Users;

public sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (Request request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.CPF,
                request.BirthDate,
                request.SuperiorId,
                request.Address,
                request.Contatos
                ));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.AddUser)
        .WithTags(Tags.Users);
    }

    public sealed class Request
    {
        public string Email { get; init; }

        public string Password { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string CPF { get; init; }

        public string Address { get; init; }

        public DateTime BirthDate { get; init; }

        public Guid? SuperiorId { get; init; }
        public List<string> Contatos { get; init; }
    }
}
