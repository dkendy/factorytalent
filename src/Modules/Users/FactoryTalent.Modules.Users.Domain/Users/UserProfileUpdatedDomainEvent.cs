using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;

public sealed class UserProfileUpdatedDomainEvent(Guid userId, string firstName, string lastName, string address) : DomainEvent
{
    public Guid UserId { get; init; } = userId;

    public string FirstName { get; init; } = firstName;

    public string LastName { get; init; } = lastName;

    public string Address { get; init; } = address;
}
