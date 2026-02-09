using System.Collections.Generic;
using System.Threading;

namespace Runtime.Command.Base
{
    /// <summary>
    /// Defines a contract for a command executor that can run sequences of commands in a given context.
    /// </summary>
    /// <typeparam name="ContextT">Type of the object/context that the commands operate on </typeparam>
    public interface ICommandExecutor<ContextT>
    {
        /// <summary>
        /// Returns true if the executor is currently running a command sequence.
        /// </summary>
        public bool IsCommandExecuting { get; }

        /// <summary>
        /// Executes a sequence of commands with an optional external cancellation token.
        /// Cancels any currently running command chain.
        /// </summary>
        /// <param name="commands">List of commands to execute</param>
        /// <param name="externalCT">Optional cancellation token from outside</param>
        public void ExecuteCommand(List<ICommand<ContextT>> commands, CancellationToken externalCT = default);

        /// <summary>
        /// Cancels any currently executing commands and resets the executor state.
        /// </summary>
        public void CancelCurrentCommandExecution();
    }
}