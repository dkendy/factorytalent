using FactoryTalent.Common.Domain;
using MediatR;

namespace FactoryTalent.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
