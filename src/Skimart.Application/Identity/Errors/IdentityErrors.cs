namespace Skimart.Application.Identity.Errors;

public static class IdentityErrors
{
    public static string LoginFailed => "Login failed for the user, wrong email or password.";
    public static string UserAlreadyExists => "User with given email already exists.";
    public static string UserNotFoundOnToken => "User not found on given token.";
    public static string UserNotFoundOnAuthService => "User not found.";
    public static string RegistrationFailed => "Registration failed for the user.";
    public static string UpdatingAddressFailed => "Failed to update address.";
}