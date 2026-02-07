using Cysharp.Threading.Tasks;
using Runtime.Agents;
using System.Threading;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    public class LookAtCommand : AgentCommandBase
    {
        public async override UniTask Execute(Agent context, CancellationToken token)
        {
            while (!token.IsCancellationRequested && context.DetectionModule.Target != null) 
            {
                Vector3 direction = (context.DetectionModule.Target.position - context.LocomotionModule.LocomotionRoot.position).normalized;
                context.LocomotionModule.RotateTowardsDirection(direction);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
