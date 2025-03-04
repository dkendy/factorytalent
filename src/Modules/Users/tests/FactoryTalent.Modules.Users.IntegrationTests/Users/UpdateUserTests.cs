using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Users.RegisterUser;
using FactoryTalent.Modules.Users.Application.Users.UpdateUser;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class UpdateUserTests : BaseIntegrationTest
{
    public UpdateUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<UpdateUserCommand> InvalidCommands = new()
    {
        new UpdateUserCommand(Guid.Empty, Faker.Name.FirstName(), Faker.Name.LastName(),  string.Empty, "-"),
        new UpdateUserCommand(Guid.NewGuid(), "", Faker.Name.LastName(),  string.Empty,string.Empty),
        new UpdateUserCommand(Guid.NewGuid(), Faker.Name.FirstName(), "", string.Empty, "-")
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnError_WhenCommandIsNotValid(UpdateUserCommand command)
    {
        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(userId, Faker.Name.FirstName(), Faker.Name.LastName(), string.Empty, string.Empty));

        // Assert
        updateResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password()+ "P@$$0w0rd123",
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            "46601060046",
            DateTime.Now.AddYears(-19),
            null,
            "",
            new List<string>()));

        Guid userId = result.Value;

        // Act
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(userId, Faker.Name.FirstName(), Faker.Name.LastName(), string.Empty, "-"));

        // Assert
        updateResult.IsSuccess.Should().BeTrue();
    }
}
