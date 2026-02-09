using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Simple physics-based implementation of the agent locomotion module.
    /// Uses Rigidbody movement for translation and rotation to ensure proper interaction with Unity's physics system.
    /// Movement and rotation methods should be called repeatedly until completion.
    /// </summary>
    public class AgentLocomotionModuleSimple : MonoBehaviour, IAgentLocomotionModule
    {
        /// <summary>
        /// Rigidbody responsible for movement and rotation.
        /// Movement is performed using physics-safe methods (MovePosition / MoveRotation).
        /// </summary>
        [SerializeField] private Rigidbody m_Rigidbody;

        /// <summary>
        /// Root transform used for positional movement.
        /// In this simple implementation, it is the same as the Rigidbody transform.
        /// </summary>
        public Transform LocomotionRoot => m_Rigidbody.transform;

        /// <summary>
        /// Pivot used for rotation control.
        /// Separated in interface to allow more complex setups, but currently mapped to Rigidbody transform.
        /// </summary>
        public Transform RotationPivot => m_Rigidbody.transform;

        /// <summary>
        /// Linear movement speed (units per second).
        /// </summary>
        [field: SerializeField] public float Speed { get; private set; } = 10f;

        /// <summary>
        /// Distance threshold used to determine when movement has reached target.
        /// Prevents oscillation and precision issues.
        /// </summary>
        [field: SerializeField, Range(float.Epsilon, 1)] public float PositionThreshold { get; private set; } = 0.05f;

        /// <summary>
        /// Angular rotation speed (degrees per second).
        /// </summary>
        [field: SerializeField, Range(float.Epsilon, 180)] public float AngularSpeed { get; private set; } = 10f;

        /// <summary>
        /// Angular threshold used to determine when rotation is complete.
        /// </summary>
        [field: SerializeField, Range(float.Epsilon, 1)] public float AngularThreshold { get; private set; } = 0.05f;

        /// <summary>
        /// Moves the Rigidbody toward a world-space target position.
        /// Uses Rigidbody.MovePosition to maintain physics compatibility.
        /// Returns:
        /// - true when the agent is within PositionThreshold of the target.
        /// - false while movement is still in progress.
        /// </summary>
        public bool MoveTowardsTarget(Vector3 target) 
        {
            float speed = Time.deltaTime * Speed;
            m_Rigidbody.MovePosition(Vector3.MoveTowards(LocomotionRoot.position, target, speed));

            return Vector3.Distance(LocomotionRoot.position, target) <= PositionThreshold;
        }

        /// <summary>
        /// Rotates the Rigidbody toward a desired forward direction.
        /// Uses Quaternion.RotateTowards for smooth incremental rotation.
        /// Returns:
        /// - true when rotation is within AngularThreshold.
        /// - false while rotation is still in progress.
        /// </summary>
        public bool RotateTowardsDirection(Vector3 direction)
        {
            Quaternion desired = Quaternion.LookRotation(direction);
            float speed = Time.deltaTime * AngularSpeed;
            m_Rigidbody.MoveRotation(Quaternion.RotateTowards(RotationPivot.rotation, desired, speed));

            return Quaternion.Angle(RotationPivot.rotation, desired) <= AngularThreshold;
        }
    }
}
