using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Auth.Dtos;
using static Skimart.Domain.AppConstants;

namespace Skimart.Application.Cases.Auth.Commands.Register;

public record RegisterCommand(
    [Required] 
    string DisplayName,
    [Required]
    [EmailAddress]
    string Email, 
    [Required]
    [RegularExpression(Pwd.Regex, ErrorMessage = "Password must have one uppercase, " +
                                                              "one lowercase, one number and one " +
                                                              "non-alphanumeric and at least 6 characters")]
    string Password) : IRequest<Result<UserDto>>;