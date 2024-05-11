using ErrorOr;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Basket.Mappers;
using Skimart.Application.Gateways.Memory.Basket;
using Skimart.Domain.Entities.Basket;
using Error = ErrorOr.Error;

namespace Skimart.Application.Basket.Commands.CreateOrUpdateBasket;

public class CreateOrUpdateBasketHandler : IRequestHandler<CreateOrUpdateBasketCommand, ErrorOr<CustomerBasket>>
{
    private readonly ILogger<CreateOrUpdateBasketHandler> _logger;
    private readonly IBasketRepository _basketRepos;

    public CreateOrUpdateBasketHandler(
        ILogger<CreateOrUpdateBasketHandler> logger, 
        IBasketRepository basketRepos)
    {
        _logger = logger;
        _basketRepos = basketRepos;
    }
    
    public async Task<ErrorOr<CustomerBasket>> Handle(CreateOrUpdateBasketCommand command, CancellationToken cancellationToken)
    {
        var basketToChange = command.ToDomain();
        
        var resultingCustomerBasket = await _basketRepos.CreateOrUpdateBasketAsync(basketToChange);

        return resultingCustomerBasket is not null ? 
            resultingCustomerBasket :
            Error.Failure(description: "Failed to create or update the basket.");
    }
}