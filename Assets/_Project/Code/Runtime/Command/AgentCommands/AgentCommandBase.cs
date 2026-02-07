using Cysharp.Threading.Tasks;
using Runtime.Agents;
using Runtime.Command.Base;
using System.Threading;

namespace Runtime.Command.AgentCommands
{
    //TO DO Move to ScripableObject
    public abstract class AgentCommandBase : ICommand<Agent>
    {
        public abstract UniTask Execute(Agent context, CancellationToken cancel);
    }
}