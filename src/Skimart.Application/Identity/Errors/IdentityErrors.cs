namespace Skimart.Application.Identity.Errors;

public static class IdentityErrors
{
    public static string LoginFailed => "Login failed for the user, wrong email or password.";
    public static string UserAlreadyExists => "User with given email already exists.";
    public static string RegistrationFailed => "Registration failed for the user.";
    public static string UserFromTokenNotFound => "User from token not found.";
    public static string UpdatingAddressFailed => "Failed to update address.";
}