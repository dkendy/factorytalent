﻿using FactoryTalent.Common.Application.EventBus;
using FactoryTalent.Common.Application.Exceptions;
using FactoryTalent.Common.Application.Messaging;
using FactoryTalent.Common.Domain;
using FactoryTalent.Modules.Users.Application.Users.GetUser;
using FactoryTalent.Modules.Users.Domain.Users;
using FactoryTalent.Modules.Users.IntegrationEvents;
using MediatR;

namespace FactoryTalent.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus bus)
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<UserResponse> result = await sender.Send(
            new GetUserQuery(domainEvent.UserId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new FactoryTalentException(nameof(GetUserQuery), result.Error);
        }

        await bus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName,
                result.Value.CPF,
                result.Value.Address),
            cancellationToken);
    }
}
