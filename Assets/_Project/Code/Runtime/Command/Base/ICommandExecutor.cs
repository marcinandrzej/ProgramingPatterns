using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Runtime.Command.Base
{
    public interface ICommandExecutor<ContextT>
    {
        public bool IsCommandExecuting { get; }

        public void ExecuteCommand(List<ICommand<ContextT>> commands, CancellationToken externalCT = default);

        public void CancelCurrentCommandExecution();
    }
}