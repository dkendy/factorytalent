using Bogus;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.UnitTests.Abstractions; 
using FluentAssertions;


namespace FactoryTalent.Modules.Users.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnUser()
    { 

        // Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-18),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldReturnUser_WithMemberRole()
    {
        // Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-18),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());

        // Assert
        user.Roles.Single().Should().Be(Role.Member);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        // Act
        var user = User.Create(
             Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-18),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());
        // Assert
        UserRegisteredDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        // Arrange
        var user = User.Create(
             Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
             "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-18),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());

        // Act
        user.Update(user.LastName, user.FirstName, Faker.Address.FullAddress());

        // Assert
        UserProfileUpdatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserProfileUpdatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.FirstName.Should().Be(user.FirstName);
        domainEvent.LastName.Should().Be(user.LastName);
        domainEvent.Address.Should().Be(user.Address);
    }

    [Fact]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        // Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
             "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-18),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());

        user.ClearDomainEvents();

        // Act
        user.Update(user.FirstName, user.LastName, Faker.Address.FullAddress());

        // Assert
        user.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Create_ShouldRiseErro_WhenUserNotBirthday_18()
    {
        // Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
             "75623986072",
            Faker.Address.FullAddress(),
            DateTime.Now.AddYears(-17),
            null,
            Guid.NewGuid().ToString(),
            new List<string>());

        user.ClearDomainEvents();

        // Act
        user.Update(user.FirstName, user.LastName, Faker.Address.FullAddress());

        // Assert
        user.DomainEvents.Should().BeEmpty();
    }
}
