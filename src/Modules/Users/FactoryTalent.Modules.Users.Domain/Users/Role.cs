namespace FactoryTalent.Modules.Users.Domain.Users;

public sealed class Role
{
    public static readonly Role Administrator = new("Administrator", 4);
    public static readonly Role Manager = new("Managers",3);
    public static readonly Role Employee = new("Employees",2);
    public static readonly Role Intern = new("Intern",1);


    private Role(string name, int level)
    {
        Name = name;
        Level = level;
    }

    private Role()
    {
    }

    public string Name { get; private set; }

    public int Level { get; private set; }
}
