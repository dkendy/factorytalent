using FactoryTalent.Common.Application.EventBus;

namespace FactoryTalent.Modules.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public UserRegisteredIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string cpf,
        string address)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        CPF = cpf;
        Address = address;

    }

    public Guid UserId { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string CPF { get; init; }

    public string Address { get; init; }
     


}
