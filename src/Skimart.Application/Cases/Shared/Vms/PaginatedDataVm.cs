namespace Skimart.Application.Cases.Shared.Vms;

public record PaginatedDataVm<T>(int PageIndex, int PageSize, int Count, IReadOnlyList<T> Data) where T : class;