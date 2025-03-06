using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Abstractions.Helper;
using FactoryTalent.Modules.Users.Application.Users.GetUser;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions;  
using FluentAssertions;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class GetUserTests : BaseIntegrationTest
{
    public GetUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result<UserResponse> userResult = await Sender.Send(new GetUserQuery(userId));

        // Assert
        userResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(
            new RegisterUserCommand(Faker.Internet.Email(),
            Faker.Internet.Password() + "P@$$0w0rd123", 
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            CpfGenerator.Create(),
            DateTime.Now.AddYears(-18),
            null,
            string.Empty,
            new List<string>(), Role.Administrator));
        Guid userId = result.Value;

        // Act
        Result<UserResponse> userResult = await Sender.Send(new GetUserQuery(userId));

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();
    }
}
