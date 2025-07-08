using BuildingBlocks.Commons;
using Identity.Application.Errors;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.Commands;

public sealed record UpdateUserProfileCommand(int UserId, string FullName, string Description) : ICommand;

public class UpdateUserProfileCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserProfileCommand>
{
    public async Task<Result> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if(user == null) return Result.Failure(UserErrors.UserNotFound);
        
        user.FullName = command.FullName;
        user.Description = command.Description;
        
        var result = await userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(e.Code, e.Description));
            return Result.Failure(new Error("Update User Error", string.Join("\n", errors)));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}