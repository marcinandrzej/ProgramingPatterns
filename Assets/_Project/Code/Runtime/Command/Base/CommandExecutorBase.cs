using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Command.Base
{
    /// <summary>
    /// Base implementation of a command executor using UniTask and cancellation tokens.
    /// Handles execution, cancellation, and lifetime of command sequences for a given context.
    /// </summary>
    /// <typeparam name="ContextT">Type of the context object.</typeparam>
    public abstract class CommandExecutorBase<ContextT> : MonoBehaviour, ICommandExecutor<ContextT>
    {
        // Context instance for commands
        [SerializeField] private ContextT context;

        // Tracks current command sequence
        private CancellationTokenSource executionCTS = null;

        private bool isRunning;

        /// <summary>
        /// Indicates whether a command sequence is currently being executed.
        /// </summary>
        public bool IsCommandExecuting => isRunning;

        /// <summary>
        /// Executes a list of commands asynchronously.
        /// Automatically cancels any running sequence and links to an optional external cancellation token.
        /// </summary>
        public void ExecuteCommand(List<ICommand<ContextT>> commands, CancellationToken externalCT = default)
        {
            // Cancel previous execution
            CancelCurrentCommandExecution();

            isRunning = true;

            // Create a linked cancellation token combining the MonoBehaviour destruction token and an optional external token
            executionCTS = externalCT == default ?
                CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy()) :
                CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy(), externalCT);

            // Fire and forget the async command execution, suppressing OperationCanceledException
            Run(commands, executionCTS.Token).SuppressCancellationThrow().Forget();
        }

        /// <summary>
        /// Cancels any currently executing commands and disposes the CTS.
        /// Resets running flag.
        /// </summary>
        public void CancelCurrentCommandExecution()
        {
            if (executionCTS != null)
            {
                executionCTS?.Cancel();
                executionCTS?.Dispose();
                executionCTS = null;
            }

            isRunning = false;
        }


        /// <summary>
        /// Executes commands sequentially, yielding each frame until completion or cancellation.
        /// </summary>
        private async UniTask Run(List<ICommand<ContextT>> commands, CancellationToken token)
        {
            // Capture local CTS to avoid race conditions
            CancellationTokenSource localCTS = executionCTS;

            try
            {
                foreach (var command in commands)
                    await command.Execute(context, token);
            }
            finally
            {
                // Only dispose if this is still the active CTS
                if (executionCTS == localCTS)
                {
                    executionCTS?.Dispose();
                    executionCTS = null;
                    isRunning = false;
                }
            }
        }
    }
}
