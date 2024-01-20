using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Extensions.Request;

public static class HttpRequestExtensions
{
    public static HttpRequestDto ToDto(this HttpRequest request) => new HttpRequestDto(request.Path, request.Query);
}