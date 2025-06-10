namespace BuildingBlocks.ResultPattern;

public class Result(bool isSuccess, Error error)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } = error;
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T>(bool isSuccess, T value, Error error) : Result(isSuccess, error)
{
    public T Value { get; } = value;
    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static Result<T> Failure(Error error) => new(false, default!, error);
}

public class Error(string? code, string? description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static implicit operator Result(Error error) => Result.Failure(error);
   }

