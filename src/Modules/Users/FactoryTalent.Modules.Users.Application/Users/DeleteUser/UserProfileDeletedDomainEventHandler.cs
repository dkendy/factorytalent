using FactoryTalent.Common.Application.EventBus;
using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.IntegrationEvents;

namespace FactoryTalent.Modules.Users.Application.Users.UpdateUser;

internal sealed class UserProfileDeletedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<UserProfileDeletedDomainEvent>
{
    public override async Task Handle(
        UserProfileDeletedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new UserProfileDeletedIntegrationEvent(domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.UserId),
            cancellationToken);
    }


}
