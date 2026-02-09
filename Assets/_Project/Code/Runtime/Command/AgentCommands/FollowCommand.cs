using Cysharp.Threading.Tasks;
using Runtime.Agents;
using System.Threading;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    /// <summary>
    /// Command for agent to follow a target while maintaining a safe distance.
    /// Continuously rotates toward and moves toward the target.
    /// </summary>
    public class FollowCommand : AgentCommandBase
    {
        private const float Distance = 3f;

        public async override UniTask Execute(Agent context, CancellationToken token)
        {
            while (!token.IsCancellationRequested && context.DetectionModule.Target != null)
            {
                Vector3 direction = (context.DetectionModule.Target.position - context.LocomotionModule.LocomotionRoot.position);
                Vector3 directionNormalized = direction.normalized;
                context.LocomotionModule.RotateTowardsDirection(directionNormalized);

                Vector3 directionProjected = Vector3.ProjectOnPlane(direction, Vector3.up);
                Vector3 target = context.LocomotionModule.LocomotionRoot.position + directionProjected;

                if (directionProjected.magnitude > Distance)
                    context.LocomotionModule.MoveTowardsTarget(target);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
