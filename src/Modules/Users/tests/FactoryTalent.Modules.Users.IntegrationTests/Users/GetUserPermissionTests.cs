using FactoryTalent.Common.Application.Authorization;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Users.GetUserPermissions;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class GetUserPermissionTests : BaseIntegrationTest
{
    public GetUserPermissionTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        string identityId = Guid.NewGuid().ToString();

        // Act
        Result<PermissionsResponse> permissionsResult = await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.Error.Should().Be(UserErrors.NotFound(identityId));
    }

    [Fact]
    public async Task Should_ReturnPermissions_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(
            new RegisterUserCommand( Faker.Internet.Email(),
             Faker.Internet.Password() +"P@$$0w0rd123",
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            "96896932040",
            DateTime.Now.AddYears(-18), 
            null,
            string.Empty,
            new List<string>()));

        string identityId = DbContext.Users.Where(a => a.Id == result.Value).First().IdentityId;

        // Act
        Result<PermissionsResponse> permissionsResult = await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.IsSuccess.Should().BeTrue();
        permissionsResult.Value.Permissions.Should().NotBeEmpty();
    }
}
