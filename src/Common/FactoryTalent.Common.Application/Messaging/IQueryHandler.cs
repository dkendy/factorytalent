﻿using FactoryTalent.Common.Domain;
using MediatR;

namespace FactoryTalent.Common.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
