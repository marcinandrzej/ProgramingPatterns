using System.Collections.Generic;

namespace Runtime.Command.Base
{
    public interface ICommandExecutor<ContextT>
    {
        public bool IsCommandExecuting { get; }

        public void ExecuteCommand(List<ICommand<ContextT>> commands);
    }
}