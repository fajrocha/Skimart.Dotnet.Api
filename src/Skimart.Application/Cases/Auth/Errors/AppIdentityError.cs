using FluentResults;

namespace Skimart.Application.Cases.Auth.Errors;

public class AppIdentityError : Error
{
    private AppIdentityError(string message) : base(message)
    { 
    }

    public static AppIdentityError NoLoggedUser => new("Could not get logged user.");
    public static AppIdentityError AddressNotFound => new("Address not found for the user.");
    public static AppIdentityError AddressUpdateFailed => new("Address update failed for the user.");
    public static AppIdentityError UserNotFound => new("User not found.");
    public static AppIdentityError UserRegistrationFailed => new("Registration failed for the user.");
    public static AppIdentityError UserLoginFailed => new("Login failed for the user, wrong email or password.");
}