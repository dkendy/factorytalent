using FactoryTalent.Common.Domain;

namespace FactoryTalent.Modules.Users.Domain.Users;

public sealed class UserProfileDeletedDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;

}
