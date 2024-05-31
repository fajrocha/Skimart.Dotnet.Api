using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Basket.Errors;
using Skimart.Application.Basket.Gateways;

namespace Skimart.Application.Basket.Commands.DeleteBasket;

public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, ErrorOr<Deleted>>
{
    private readonly ILogger<DeleteBasketHandler> _logger;
    private readonly IBasketRepository _basketRepos;

    public DeleteBasketHandler(ILogger<DeleteBasketHandler> logger, IBasketRepository basketRepos)
    {
        _logger = logger;
        _basketRepos = basketRepos;
    }
    
    public async Task<ErrorOr<Deleted>> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        return await _basketRepos.DeleteBasketAsync(command.Id) ? 
            Result.Deleted : 
            Error.Failure(description: BasketErrors.FailedToDeleteBasket);
    }
}