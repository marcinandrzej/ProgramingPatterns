using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Command.Base
{
    public abstract class CommandExecutorBase<ContextT> : MonoBehaviour, ICommandExecutor<ContextT>
    {
        [SerializeField] private ContextT context;

        private CancellationTokenSource executionCTS = null;

        private bool isRunning;
        public bool IsCommandExecuting => isRunning;

        public void ExecuteCommand(List<ICommand<ContextT>> commands, CancellationToken externalCT = default)
        {
            CancelCurrentCommandExecution();

            isRunning = true;
            executionCTS = externalCT == default ?
                CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy()) :
                CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy(), externalCT);
            Run(commands, executionCTS.Token).SuppressCancellationThrow().Forget();
        }

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

        private async UniTask Run(List<ICommand<ContextT>> commands, CancellationToken token)
        {
            CancellationTokenSource localCTS = executionCTS;

            try
            {
                foreach (var command in commands)
                    await command.Execute(context, token);
            }
            finally
            {
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
