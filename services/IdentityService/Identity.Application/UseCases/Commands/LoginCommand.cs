using BuildingBlocks.Commons;
using Identity.Application.Commons;
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
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByNameAsync(command.UserName);
        if (user == null)
            return Result<LoginResponse>.Failure(AuthenErrors.UserNotFound);

        var isPasswordValid = await userRepository.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
            return Result<LoginResponse>.Failure(AuthenErrors.WrongPassword);

        var roles = await userRepository.GetRolesAsync(user);
        var accessToken = jwtService.GenerateJwtToken(user, string.Join(",", roles));
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redisService.SetValue(refreshKey, refreshToken, TimeSpan.FromDays(jwtService.getRefreshTokenValidity()));
            await redisService.SetValue(accessKey, accessToken, TimeSpan.FromMinutes(jwtService.getAccessTokenValidity()));
        }
        catch (Exception ex)
        {
            return Result<LoginResponse>.Failure(new Error("Redis.SaveFailed", $"Failed to save tokens: {ex.Message}"));
        }
        
        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }
}