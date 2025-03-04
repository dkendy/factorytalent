using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;

public sealed class User: Entity
{
    private readonly List<Role> _roles = [];

    private readonly List<Contact> _contacts = [];

    private User()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string CPF { get; private set; }

    public string? Address { get; private set; }

    public DateTime? BirthDay { get; private set; }

    public Guid? SuperiorId { get; private set; }
    public User? Superior { get; private set; }

    public ICollection<User> Subordinates { get; private set; } = new List<User>(); 

    public string IdentityId { get; private set; }

    public IReadOnlyCollection<Contact> Contacs => _contacts.ToList();

    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public static User Create(string email, string firstName, string lastName, string CPF, string address, DateTime birthDay, Guid? userIdUpper, string identityId, List<string> contacts)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityId = identityId,
            CPF = CPF,
            Address = address,
            BirthDay = birthDay.ToUniversalTime(),
            SuperiorId = userIdUpper
        };

        user._roles.Add(Role.Member);

        List<Contact> _contacts = new();
        contacts.ForEach((s) =>
        {
            _contacts.Add(Contact.Create( s, user.Id ));
        });
        

        user._contacts.AddRange(_contacts);

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void Update(string firstName, string lastName, string address)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;
        Address = address;

        Raise(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName, Address));
    }
}
