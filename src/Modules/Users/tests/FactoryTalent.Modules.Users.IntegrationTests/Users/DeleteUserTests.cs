using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using FactoryTalent.Modules.Users.IntegrationTests.Abstractions;
using FactoryTalent.Modules.Users.Presentation.Users;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;
using FactoryTalent.Modules.Users.Application.Abstractions.Helper;
using Bogus.Bson;

namespace FactoryTalent.Modules.Users.IntegrationTests.Users;

public class DeleteUserTests : BaseIntegrationTest
{
    public DeleteUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }


    private StringContent ConvertToJson(RegisterUser.Request request)
    {
        var objrequest = new
        {
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.CPF,
            request.BirthDate,
            Contatos = new List<string>(),
            request.Address,
            Role = "Administrator"
        }; 
        return new StringContent(JsonConvert.SerializeObject(objrequest), Encoding.UTF8, "application/json"); 
    }

     
    [Fact]
    public async Task Should_ReturnNoContent_WhenRequestIsValidForDelete()
    {
        // Arrange 
        using StringContent request = ConvertToJson(new RegisterUser.Request
        {
            Email = "create@test.com",
            Password = "P@$$0w0rd123",
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            BirthDate = DateTime.Now.AddYears(-18),
            CPF = "89943385006",
            Address = Faker.Address.FullAddress(),
            Contatos = new List<string>()
        }); 

        // Act
        string token = await GetAccessTokenAsync();

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await HttpClient.PostAsync("users/register", request);

        string responseBody = await response.Content.ReadAsStringAsync(); 

        HttpResponseMessage responsedelete = await HttpClient.DeleteAsync($"users/profile/{responseBody.Trim('"')}");

        // Assert
        responsedelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
 
}
