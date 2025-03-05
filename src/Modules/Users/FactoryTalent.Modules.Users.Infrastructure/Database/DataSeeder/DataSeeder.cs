 
using System.Net.Http.Headers; 
using FactoryTalent.Modules.Users.Domain.Users; 
using Microsoft.Extensions.Configuration; 
using FactoryTalent.Modules.Users.Application.Abstractions.Data;
using Newtonsoft.Json;
using System.Text;
using FactoryTalent.Modules.Users.Infrastructure.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FactoryTalent.Modules.Users.Application.Abstractions.Helper;

namespace FactoryTalent.Modules.Users.Infrastructure.Database.DataSeeder;

public class DataSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _provider;


    public DataSeeder(IServiceProvider provider, IUserRepository userRepository,  HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
        _provider = provider;
    }

    public async Task SeedAsync()
    {

        KeyCloakOptions keycloakOptions = _provider
                   .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

        string? keycloakUrl = keycloakOptions.AdminUrl.Split("/admin")[0];

        string? realm = "factory";

        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", "factory-confidential-client" },
                { "client_secret","PzotcrvZRF9BHCKcUxdKfHWlIPECG49k" },
                { "scope" , "openid" }
            });

        HttpResponseMessage tokenResponse = await _httpClient.PostAsync($"{keycloakUrl}/realms/{realm}/protocol/openid-connect/token", content
            );

        if (!tokenResponse.IsSuccessStatusCode)
        {   
            throw new Exception("Erro ao obter token de admin do Keycloak.");
        }

        string tokenJson = await tokenResponse.Content.ReadAsStringAsync();
        Dictionary<string, object>? tokenObj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(tokenJson);
        string accessToken = tokenObj?["access_token"].ToString();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


        var userData = new
        {
            username = keycloakOptions.UserAdmin,
            emailVerified = true,
            firstName = "Adm",
            lastName = "Adm",
            email = keycloakOptions.UserAdmin,
            enabled = true,
            credentials = new[]
            {
                new { type = "password", value = keycloakOptions.PasswordAdm, temporary = false }
            }
        };

        string jsonString = JsonConvert.SerializeObject(userData);

        using var userContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync($"{keycloakUrl}/admin/realms/{realm}/users", userContent);

         
        HttpResponseMessage usersIdResponse = await _httpClient.GetAsync($"{keycloakUrl}/admin/realms/{realm}/users?username={keycloakOptions.UserAdmin}");

        if (!usersIdResponse.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao buscar usuário no Keycloak.");
        }

        string usersJson = await usersIdResponse.Content.ReadAsStringAsync();

        List<KeycloakUser>? users = System.Text.Json.JsonSerializer.Deserialize<List<KeycloakUser>>(usersJson);

        if (users == null || users.Count == 0)
        {
            throw new Exception("Usuário não encontrado no Keycloak.");
        }

        User? _userRegsitered = await _userRepository.GetByEmailAsync(userData.email);

        if(_userRegsitered == null)
        {
            _userRepository.Insert(User.Create(userData.email, userData.firstName, userData.lastName, CpfGenerator.Create(), string.Empty, DateTime.Now, null, users[0].id, new List<string>()));
            await _unitOfWork.SaveChangesAsync();
        }

 



    }
}

public class KeycloakUser
{
    public string id { get; set; }
}


