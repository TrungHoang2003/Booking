namespace BuildingBlocks.Commons;

public class Result(bool success, Error? error)
{
    public bool Success { get; } = success;
    public Error? Error { get; } = error;
    public static Result IsSuccess() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T>(bool success, T data, Error? error) : Result(success, error)
{
    public T Data { get; } = data;
    public static Result<T> IsSuccess(T value) => new(true, value, null);
    public new static Result<T> Failure(Error error) => new(false, default!, error);
}

public class Error(string? code, string? description)
{
    public string? Code { get; } = code;
    public string? Description { get; } = description;
    public static implicit operator Result(Error error) => Result.Failure(error);
}

