using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Errors;
using Identity.Infrastructure.Repositories;

namespace Identity.Application.UseCases.Commands;

public sealed record LoginCommand(
    string UserName,
    string Password
    ) : ICommand<LoginResponse>;

public sealed record LoginResponse
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; } 
}

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IRedisService redisService)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<ResultPattern<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByNameAsync(command.UserName);
        if (user == null)
            return ResultPattern<LoginResponse>.Failure(AuthenErrors.UserNotFound);

        var isPasswordValid = await userRepository.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
            return ResultPattern<LoginResponse>.Failure(AuthenErrors.WrongPassword);

        var roles = await userRepository.GetRolesAsync(user);
        var accessToken = jwtService.GenerateJwtToken(user, string.Join(",", roles));
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redisService.SetValue(refreshKey, refreshToken, TimeSpan.FromDays(jwtService.GetRefreshTokenValidity()));
            await redisService.SetValue(accessKey, accessToken, TimeSpan.FromMinutes(jwtService.GetAccessTokenValidity()));
        }
        catch (Exception ex)
        {
            return ResultPattern<LoginResponse>.Failure(new Error("Redis.SaveFailed", $"Failed to save tokens: {ex.Message}"));
        }
        
        return ResultPattern<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }
}