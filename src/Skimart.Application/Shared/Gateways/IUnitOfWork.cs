using Skimart.Domain.Entities;

namespace Skimart.Application.Shared.Gateways;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}