using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;
using Skimart.Application.Identity.DTOs;
using static Skimart.Domain.AppConstants;

namespace Skimart.Application.Identity.Commands.Register;

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