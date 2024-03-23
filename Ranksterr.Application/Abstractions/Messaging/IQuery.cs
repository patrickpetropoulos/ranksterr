using MediatR;
using Ranksterr.Domain.Abstractions;

namespace Ranksterr.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}