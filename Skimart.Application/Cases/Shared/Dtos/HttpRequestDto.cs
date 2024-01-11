using Microsoft.AspNetCore.Http;

namespace Application.Cases.Shared.Dtos;

public record HttpRequestDto(PathString Path, IQueryCollection Query)
{
    public void Deconstruct(out PathString path, out IQueryCollection query)
    {
        path = Path;
        query = Query;
    }
};