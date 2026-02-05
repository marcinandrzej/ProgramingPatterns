using Runtime.Agents;
using Runtime.Command.Base;
using System.Threading.Tasks;

namespace Runtime.Command.AgentCommands
{
    public abstract class AgentCommandBase : ICommand<Agent>
    {
        public abstract Task Execute(Agent context);
    }
}