using ErrorOr;
using MediatR;

namespace Skimart.Application.Shared;

public interface IResultRequest<TResponse> : IRequest<ErrorOr<TResponse>>
{
}