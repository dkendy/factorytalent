using FactoryTalent.Common.Application.EventBus;

namespace FactoryTalent.Modules.Users.IntegrationEvents;

public sealed class UserProfileDeletedIntegrationEvent : IntegrationEvent
{
    public UserProfileDeletedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
   
}
