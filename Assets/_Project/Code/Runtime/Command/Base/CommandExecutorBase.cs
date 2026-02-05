using UnityEngine;
using System.Collections.Generic;

namespace Runtime.Command.Base
{
    public abstract class CommandExecutorBase<ContextT> : MonoBehaviour, ICommandExecutor<ContextT>
    {
        [SerializeField] private ContextT context;
        private bool isCommandExecuting = false;

        public bool IsCommandExecuting => isCommandExecuting;

        public async void ExecuteCommand(List<ICommand<ContextT>> commands)
        {
            isCommandExecuting = true;

            foreach (ICommand<ContextT> command in commands)
                await command.Execute(context);

            isCommandExecuting = false;
        }
    }
}