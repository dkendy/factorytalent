
using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;
public sealed class Contact : Entity
{

    private Contact()
    {
    }
    public Guid Id { get; private set; }

    public string PhoneNumber { get; private set; }
    public Guid UserId { get; private set; }

    public static Contact Create(string phoneNumer, Guid UserId)
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            PhoneNumber = phoneNumer,
            UserId = UserId
        };

       
        return contact;
    }

}
