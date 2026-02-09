using Cysharp.Threading.Tasks;
using Runtime.Agents;
using System.Threading;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    /// <summary>
    /// Command for agent to search the environment.
    /// Rotates, moves randomly within a radius, and periodically looks around.
    /// </summary>
    public class SeekCommand : AgentCommandBase
    {
        private const int Delay = 3000;
        private const float Radius = 5f;

        public async override UniTask Execute(Agent context, CancellationToken token)
        {
            Vector3 centralPoint = context.LocomotionModule.LocomotionRoot.position;
            
            while (!token.IsCancellationRequested)
            {
                // Rotate and scan environment in multiple directions
                Vector3 forward = context.LocomotionModule.RotationPivot.forward;
                Vector3 right = context.LocomotionModule.RotationPivot.right;
                Vector3 left = -right;

                await Rotate(context, right, token);
                await UniTask.Delay(Delay, cancellationToken: token);
                await Rotate(context, forward, token);
                await UniTask.Delay(Delay/2, cancellationToken: token);
                await Rotate(context, left, token);
                await UniTask.Delay(Delay, cancellationToken: token);
                await Rotate(context, forward, token);
                await UniTask.Delay(Delay/2, cancellationToken: token);

                // Move to a random target within a radius while facing the direction
                Vector3 targetPosition = GetTarget(context.LocomotionModule.LocomotionRoot.position, centralPoint, Radius);
                Vector3 targetDirection = (targetPosition - context.LocomotionModule.LocomotionRoot.position).normalized;

                await Rotate(context, targetDirection, token);
                await Move(context, targetPosition, token);
            }
        }

        /// <summary>
        /// Moves the agent towards a target until position threshold is reached.
        /// </summary>
        private async UniTask Move(Agent context, Vector3 target, CancellationToken token)
        {
            while (!context.LocomotionModule.MoveTowardsTarget(target))
                await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        /// <summary>
        /// Rotates the agent towards a given direction until angular threshold is reached.
        /// </summary>
        private async UniTask Rotate(Agent context, Vector3 target, CancellationToken token)
        {
            while (!context.LocomotionModule.RotateTowardsDirection(target))
                await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        /// <summary>
        /// Generates a random target position within a radius around a central point, ensuring the agent moves away from its current facing direction.
        /// </summary>
        private Vector3 GetTarget(Vector3 currentPosition, Vector3 centralPoint, float radius) 
        {
            Vector3 randomDirection = Vector3.ProjectOnPlane(Random.insideUnitSphere, Vector3.up).normalized;
            Vector3 toAgentDirection = Vector3.ProjectOnPlane(currentPosition - centralPoint, Vector3.up).normalized;

            if (Vector3.Dot(randomDirection, toAgentDirection) > 0)
                randomDirection = -randomDirection;

            Vector3 targetPosition = radius * randomDirection + centralPoint;

            return targetPosition;
        }
    }
}