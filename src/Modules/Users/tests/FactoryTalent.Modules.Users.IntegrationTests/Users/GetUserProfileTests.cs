using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FactoryTalent.Modules.Users.Application.Users.GetUser;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions; 
using FactoryTalent.Modules.Users.Presentation.Users;
using FactoryTalent.Modules.Users.TestUtilities.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class GetUserProfileTests : BaseIntegrationTest
{
    public GetUserProfileTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnUnauthorized_WhenAccessTokenNotProvided()
    {
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("users/profile");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUserExists()
    {
        // Arrange
        string accessToken = await RegisterUserAndGetAccessTokenAsync("exists@test.com", "P@$$0w0rd123");
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken);

        // Act
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); 
        HttpResponseMessage response = await HttpClient.GetAsync("users/profile");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        UserResponse? user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
    }

    private async Task<string> RegisterUserAndGetAccessTokenAsync(string email, string password)
    {
        
        var request = new RegisterUser.Request
        {
            Email = email,
            Password = password,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            BirthDate = DateTime.Now.AddYears(-18),
            CPF = CpfGenerator.Create(),
            Address = Faker.Address.FullAddress(),
            Contatos = new List<string>(),
            SuperiorId = null
        };

        string _token = await GetAccessTokenAsync();

        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        await HttpClient.PostAsJsonAsync("users/register", request);

        string accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        return accessToken;
    }
}
