using MediatR;
using Ranksterr.Domain.Abstractions;

namespace Ranksterr.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
    
}