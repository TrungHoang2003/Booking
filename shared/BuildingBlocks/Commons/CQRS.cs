using BuildingBlocks.ResultPattern;
using MediatR;

namespace BuildingBlocks.Commons;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
public interface ICommand:IRequest<Result>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;


public interface IQuery<TResponse> : IRequest<Result<TResponse>>;