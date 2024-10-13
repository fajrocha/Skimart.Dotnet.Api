namespace Skimart.Contracts.Identity.Requests;

public record RegisterRequest(string DisplayName, string Email, string Password);