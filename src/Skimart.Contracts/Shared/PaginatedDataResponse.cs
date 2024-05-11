namespace Skimart.Contracts.Shared;

public record PaginatedDataResponse<T>(int PageIndex, int PageSize, int Count, IReadOnlyList<T> Data) where T : class;