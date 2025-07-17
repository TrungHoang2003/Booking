using System.Windows.Input;
using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ICommand = BuildingBlocks.Interfaces.ICommand;
using Interfaces_ICommand = BuildingBlocks.Interfaces.ICommand;

namespace Identity.Application.CQRS.Commands;

public sealed record RegisterCommand(
    string UserName,
    string FullName,
    string Email,
    string Password
):Interfaces_ICommand;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : ICommandHandler<RegisterCommand>
{
    private const string CustomerRole = "Customer";
    public async Task<ResultPattern> Handle(RegisterCommand command, CancellationToken cancellationToken)
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
            return ResultPattern.Failure(new Error("Register.Failed", string.Join(",", errors)));
        }        var roleExists = await roleRepository.RoleExistsAsync(CustomerRole);

        return ResultPattern.Success();
    }
}