using ErrorOr;
using MediatR;

namespace Skimart.Application.Shared.Result;

public interface IResultRequest<TResponse> : IRequest<ErrorOr<TResponse>>
{
}