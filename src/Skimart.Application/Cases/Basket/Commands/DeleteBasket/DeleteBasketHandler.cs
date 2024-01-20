using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Abstractions.Memory.Basket;

namespace Skimart.Application.Cases.Basket.Commands.DeleteBasket;

public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, bool>
{
    private readonly ILogger<DeleteBasketHandler> _logger;
    private readonly IBasketRepository _basketRepos;

    public DeleteBasketHandler(ILogger<DeleteBasketHandler> logger, IBasketRepository basketRepos)
    {
        _logger = logger;
        _basketRepos = basketRepos;
    }
    
    public async Task<bool> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        return await _basketRepos.DeleteBasketAsync(command.Id);
    }
}