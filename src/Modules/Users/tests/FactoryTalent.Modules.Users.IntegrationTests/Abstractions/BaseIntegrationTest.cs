using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Bogus;
using FactoryTalent.Modules.Users.Infrastructure.Database;
using FactoryTalent.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FactoryTalent.Modules.Users.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable CA1012 // Abstract types should not have public constructors
public abstract class BaseIntegrationTest : IDisposable
#pragma warning restore CA1012 // Abstract types should not have public constructors
#pragma warning restore CA1515 // Consider making public types internal
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly UsersDbContext DbContext;
    protected readonly HttpClient HttpClient;
    private readonly KeyCloakOptions _options;

#pragma warning disable S3442 // "abstract" classes should not have "public" constructors
    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
#pragma warning restore S3442 // "abstract" classes should not have "public" constructors
    {
        _scope = factory.Services.CreateScope();
        HttpClient = factory.CreateClient();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        _options = factory.Services.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
    }

    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM users.users;
            DELETE FROM users.user_roles;
            """);
    }

    protected async Task<string> GetAccessTokenAsync()
    {
        return await GetAccessTokenAsync("firstName@factory.com", "123456");
    }


    protected async Task<string> GetAccessTokenAsync(string email, string password)
    {
        using var client = new HttpClient();

        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.PublicClientId),
            new("scope", "openid"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));
        authRequest.Content = authRequestContent;

        using HttpResponseMessage authorizationResponse = await client.SendAsync(authRequest);

        authorizationResponse.EnsureSuccessStatusCode();

        AuthToken authToken = await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>();

        return authToken!.AccessToken;
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
