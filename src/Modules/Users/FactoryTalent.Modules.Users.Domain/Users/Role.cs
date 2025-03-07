using System.Text.Json.Serialization;
using System.Text.Json;

namespace FactoryTalent.Modules.Users.Domain.Users;

public sealed class Role
{
    public static readonly Role Administrator = new("Administrator", 4);
    public static readonly Role Manager = new("Manager",3);
    public static readonly Role Employee = new("Employee",2);
    public static readonly Role Intern = new("Intern",1);
 
    public static IReadOnlyList<Role> AllRoles => new[] { Administrator, Manager, Employee, Intern };
     
    public static Role FromLevel(int level) =>
        AllRoles.FirstOrDefault(r => r.Level == level) ?? throw new ArgumentException($"Invalid Role Level: {level}");
     
    public static Role FromName(string name) =>
        AllRoles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"Invalid Role Name: {name}");

    private Role(string name, int level)
    {
        Name = name;
        Level = level;
    }

    private Role() { }

    public string Name { get; private set; }

    public int Level { get; private set; }

    public static implicit operator int(Role role) => role.Level;
    public static implicit operator string(Role role) => role.Name;


}

public class RoleJsonConverter : JsonConverter<Role>
{
    public override Role Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            int level = reader.GetInt32();
            return Role.FromLevel(level);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            string? name = reader.GetString();
            if (name is null)
            {
                throw new JsonException("Role name cannot be null.");
            }
            return Role.FromName(name);
        }
       

        throw new JsonException("Invalid Role value.");
    }

    public override void Write(Utf8JsonWriter writer, Role value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteNumber("level", value.Level);
        writer.WriteEndObject();
    }
}

