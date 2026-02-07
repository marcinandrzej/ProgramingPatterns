using Cysharp.Threading.Tasks;
using Runtime.Agents;
using Runtime.Command.Base;
using System.Threading;

namespace Runtime.Command.AgentCommands
{
    public abstract class AgentCommandBase : ICommand<Agent>
    {
        public abstract UniTask Execute(Agent context, CancellationToken cancel);
    }
}