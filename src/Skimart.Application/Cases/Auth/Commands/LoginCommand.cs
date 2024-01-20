using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Auth.Dtos;

namespace Skimart.Application.Cases.Auth.Commands;

public record LoginCommand(
    [Required]
    string Email,
    [Required]
    string Password) : IRequest<Result<UserDto>>;