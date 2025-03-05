namespace FactoryTalent.Modules.Users.Infrastructure.Identity;

public sealed class KeyCloakOptions
{
    public string AdminUrl { get; set; }

    public string TokenUrl { get; set; }

    public string UserAdmin { get; set; }

    public string PasswordAdm { get; set; }

    public string ConfidentialClientId { get; set; }

    public string ConfidentialClientSecret { get; set; }

    public string PublicClientId { get; set; }

    public bool RequireHttpsMetadata { get; set; }
}
