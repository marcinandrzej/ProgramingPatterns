using Cysharp.Threading.Tasks;
using System.Threading;

namespace Runtime.Command.Base
{
    /// <summary>
    /// Defines a generic command interface.
    /// Commands operate on a specific context and support cancellation.
    /// </summary>
    /// <typeparam name="ContextT">Context type</typeparam>
    public interface ICommand<ContextT>
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="context">The object/context the command operates on</param>
        /// <param name="cancel">Cancellation token to stop execution</param>
        public UniTask Execute(ContextT context, CancellationToken cancel);
    }
}