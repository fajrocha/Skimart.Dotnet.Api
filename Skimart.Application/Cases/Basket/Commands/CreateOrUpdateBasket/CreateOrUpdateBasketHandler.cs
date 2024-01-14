using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Skimart.Application.Abstractions.Memory.Basket;
using Skimart.Application.Cases.Basket.Commands.DeleteBasket;
using Skimart.Application.Cases.Basket.Errors;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Cases.Basket.Commands.CreateOrUpdateBasket;

public class CreateOrUpdateBasketHandler : IRequestHandler<CreateOrUpdateBasketCommand, Result<CustomerBasket>>
{
    private readonly ILogger<DeleteBasketHandler> _logger;
    private readonly IBasketRepository _basketRepos;
    private readonly IMapper _mapper;

    public CreateOrUpdateBasketHandler(
        ILogger<DeleteBasketHandler> logger, 
        IBasketRepository basketRepos, 
        IMapper mapper)
    {
        _logger = logger;
        _basketRepos = basketRepos;
        _mapper = mapper;
    }
    
    public async Task<Result<CustomerBasket>> Handle(CreateOrUpdateBasketCommand command, CancellationToken cancellationToken)
    {
        var customerBasket = _mapper.Map<CustomerBasket>(command.CustomerBasketDto);

        var resultingCustomerBasket = await _basketRepos.UpdateBasketAsync(customerBasket);

        return resultingCustomerBasket is not null ? 
            Result.Ok(resultingCustomerBasket) :
            Result.Fail(CustomerBasketError.UpdateOrCreateFailed);
    }
}