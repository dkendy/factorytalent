using System.Net.Http.Json;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FactoryTalent.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    internal async Task<string> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(
            $"users?email={email}",
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
        string contentString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var jsonArray = JArray.Parse(contentString);

        if (jsonArray.Count == 0 || jsonArray[0]["id"] == null || string.IsNullOrEmpty(jsonArray[0]["id"]!.ToString()))
        {
            throw new InvalidOperationException("User ID not found or is empty.");
        }

        return jsonArray[0]["id"]!.ToString();
    }

    internal async Task DeleteUserAsync(Guid identityId, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(
            $"users/{identityId}",
            cancellationToken);
        httpResponseMessage.EnsureSuccessStatusCode();
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
}
