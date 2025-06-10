using System.Windows.Input;
using BuildingBlocks.Commons;
using BuildingBlocks.ResultPattern;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ICommand = BuildingBlocks.Commons.ICommand;

namespace Identity.Application.CQRS.Commands;

public sealed record RegisterCommand(
    string UserName,
    string FullName,
    string Email,
    string Password
):ICommand;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : ICommandHandler<RegisterCommand>
{
    private const string CustomerRole = "Customer";
    public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = command.UserName,
            FullName = command.FullName,
            Email = command.Email,
        };

        var result = await userRepository.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result.Failure(new Error("Register.Failed", string.Join(",", errors)));
        }        var roleExists = await roleRepository.RoleExistsAsync(CustomerRole);

        return Result.Success();
    }
}