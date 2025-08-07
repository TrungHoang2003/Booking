using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Interfaces_ICommand = BuildingBlocks.Interfaces.ICommand;

namespace Identity.Application.UseCases.Commands;

public sealed record RegisterCommand(
    string UserName,
    string FullName,
    string Email,
    string Password
):Interfaces_ICommand;

public class RegisterCommandHandler(
    IUserRepository userRepository)
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
        }

        await userRepository.AddToRoleAsync(user, CustomerRole);
        return Result.IsSuccess();
    }
}