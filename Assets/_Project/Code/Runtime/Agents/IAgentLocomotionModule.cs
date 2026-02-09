using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Defines the contract for agent locomotion systems.
    /// Locomotion modules are responsible for handling movement and rotation logic while exposing key transforms used by external systems.
    /// The module operates incrementally — movement and rotation methods are expected to be called repeatedly until the desired result is achieved.
    /// </summary>
    public interface IAgentLocomotionModule
    {
        /// <summary>
        /// Root transform used for positional movement of the agent.
        /// Represents the object that is translated in world space.
        /// </summary>
        public Transform LocomotionRoot { get; }

        /// <summary>
        /// Pivot transform used for rotational control.
        /// Allows rotation to be separated from root movement when needed.
        /// </summary>
        public Transform RotationPivot { get; }

        /// <summary>
        /// Moves the agent toward a target world position.
        /// Expected usage:
        /// - Called repeatedly until the method returns true.
        /// Parameters:
        /// - target: desired position in world space.
        /// Returns:
        /// - true when the agent has reached the target position
        /// - false while movement is still in progress
        /// </summary>
        public bool MoveTowardsTarget(Vector3 target);

        /// <summary>
        /// Rotates the agent toward a desired forward direction.
        /// Expected usage:
        /// - Called repeatedly until the method returns true.
        /// Parameters:
        /// - targetForward: desired forward direction in world space.
        /// Returns:
        /// - true when rotation is complete
        /// - false while rotation is still in progress
        /// </summary>
        public bool RotateTowardsDirection(Vector3 targetForward);
    }
}
