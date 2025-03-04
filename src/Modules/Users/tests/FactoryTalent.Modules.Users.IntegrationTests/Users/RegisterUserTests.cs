using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions;
using FactoryTalent.Modules.Users.Presentation.Users;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<string, string, string, string, DateTime, string, string> InvalidRequests = new()
    {
        { "", Faker.Internet.Password() + "P@$$0w0rd123", Faker.Name.FirstName(), Faker.Name.LastName(), DateTime.Now.AddYears(-10), string.Empty, "1231231" },
        { Faker.Internet.Email(), "", Faker.Name.FirstName(), Faker.Name.LastName(), DateTime.Now.AddYears(-18), string.Empty, "10086529048" },
        { Faker.Internet.Email(), "12345", Faker.Name.FirstName(), Faker.Name.LastName(), DateTime.Now.AddYears(-18), Faker.Address.FullAddress(), "89943385006" },
        { Faker.Internet.Email(), Faker.Internet.Password()+ "P@$$0w0rd123", "", Faker.Name.LastName(), DateTime.Now.AddYears(-18), Faker.Address.FullAddress(), "19943385006" },
        { Faker.Internet.Email(), Faker.Internet.Password()+ "P@$$0w0rd123", Faker.Name.FirstName(), "", DateTime.Now.AddYears(-18), Faker.Address.FullAddress(), "8943385006" }
    };


    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsNotValid(
        string email,
        string password,
        string firstName,
        string lastName,
        DateTime birthday,
        string address,
        string cpf)
    {
        // Arrange
        var request = new RegisterUser.Request
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
           
            BirthDate = birthday,
            Address = address,
            CPF = cpf,
            Contatos = new List<string>(),
            SuperiorId = null
        };



        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterUser.Request
        {
            Email = "create@test.com",
            Password = "P@$$0w0rd123",
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            BirthDate = DateTime.Now.AddYears(-18),
            CPF = "89943385006",
            Address = Faker.Address.FullAddress(),
            Contatos = new List<string>(),
            SuperiorId = null
        };

        // Act
        string token = await GetAccessTokenAsync();

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnAccessToken_WhenUserIsRegistered()
    {
        // Arrange
        var request = new RegisterUser.Request
        {
            Email = "create123@test.com",
            Password = "P@$$0w0rd123",
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            BirthDate = DateTime.Now.AddYears(-18),
            CPF = "11842657046",
            Address = Faker.Address.FullAddress(),
            Contatos = new List<string>(),
            SuperiorId = null
        };

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
        await HttpClient.PostAsJsonAsync("users/register", request);

        // Act
        string accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        // Assert
        accessToken.Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenAgeIsMinorThan18(
        string email,
        string password,
        string firstName,
        string lastName,
        DateTime birthday,
        string Address,
        string CPF)
    {
        // Arrange
        var request = new RegisterUser.Request
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthday,
            Address = Address,
            CPF = CPF,
            Contatos = new List<string>(),
            SuperiorId = null
        };



        // Act
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
