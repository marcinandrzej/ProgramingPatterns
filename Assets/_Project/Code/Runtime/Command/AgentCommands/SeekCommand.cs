using Cysharp.Threading.Tasks;
using Runtime.Agents;
using System;
using System.Threading;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    public class SeekCommand : AgentCommandBase
    {
        public async override UniTask Execute(Agent context, CancellationToken token)
        {
            Debug.Log("START");

            await UniTask.Delay(5000, cancellationToken: token);

            Debug.Log("END");
        }
    }
}