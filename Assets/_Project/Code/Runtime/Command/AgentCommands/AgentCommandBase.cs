using Cysharp.Threading.Tasks;
using Runtime.Agents;
using Runtime.Command.Base;
using System.Threading;

namespace Runtime.Command.AgentCommands
{
    /// <summary>
    /// Base abstraction for all agent commands.
    /// Provides a typed context (Agent) for commands.
    /// </summary>
    public abstract class AgentCommandBase : ICommand<Agent>
    {
        /// <summary>
        /// Executes the command on a given agent instance.
        /// </summary>
        public abstract UniTask Execute(Agent context, CancellationToken cancel);
    }
}