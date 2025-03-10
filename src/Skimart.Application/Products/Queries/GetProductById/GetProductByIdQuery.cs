﻿using Skimart.Application.Shared;
using Skimart.Application.Shared.Result;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IResultRequest<Product>
{
    public string CacheKey => $"{nameof(GetProductByIdQuery)}-Request-{DateTime.UtcNow:yyyyMMdd}";
};
