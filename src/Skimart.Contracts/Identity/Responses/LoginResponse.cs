namespace Skimart.Contracts.Identity.Responses;

public record LoginResponse(string Email, string DisplayName, string Token);