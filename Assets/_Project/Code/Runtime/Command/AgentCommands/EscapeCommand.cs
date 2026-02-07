using Cysharp.Threading.Tasks;
using Runtime.Agents;
using System.Threading;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    public class EscapeCommand : AgentCommandBase
    {
        private const float Distance = 10f;

        public async override UniTask Execute(Agent context, CancellationToken token)
        {
            Vector3 targetPosition = context.DetectionModule.Target.position;
            float currentDistance = 0;

            while (!token.IsCancellationRequested && currentDistance <= Distance)
            {
                if (context.DetectionModule.Target != null)
                    targetPosition = context.DetectionModule.Target.position;

                Vector3 direction = (context.LocomotionModule.LocomotionRoot.position - targetPosition);
                Vector3 directionNormalized = direction.normalized;
                Vector3 directionProjected = Vector3.ProjectOnPlane(direction, Vector3.up);
                Vector3 destination = context.LocomotionModule.LocomotionRoot.position + directionProjected;
                context.LocomotionModule.RotateTowardsDirection(directionNormalized);
                context.LocomotionModule.MoveTowardsTarget(destination);
                currentDistance = (context.LocomotionModule.LocomotionRoot.position - targetPosition).magnitude;

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
