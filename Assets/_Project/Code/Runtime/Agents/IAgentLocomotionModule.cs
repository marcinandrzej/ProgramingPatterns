using UnityEngine;

namespace Runtime.Agents
{
    public interface IAgentLocomotionModule
    {
        public Transform LocomotionRoot { get; }

        public Transform RotationPivot { get; }

        public bool MoveTowardsTarget(Vector3 target);
        
        public bool RotateTowardsDirection(Vector3 targetForward);
    }
}
