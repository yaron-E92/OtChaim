namespace OtChaim.Application.Common;

/// <summary>
/// Defines a handler for a specific command type.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Handles the specified command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}
