using UnityEngine;

namespace Runtime.Agents
{
    public class AgentLocomotionModuleSimple : MonoBehaviour, IAgentLocomotionModule
    {
        [SerializeField] private Rigidbody m_Rigidbody;

        public Transform LocomotionRoot => m_Rigidbody.transform;

        public Transform RotationPivot => m_Rigidbody.transform;

        //TO DO MOVE PARAMETRISATION TO COMMANDS
        [field: SerializeField] public float Speed { get; private set; } = 10f;

        [field: SerializeField, Range(float.Epsilon, 1)] public float PositionThreshold { get; private set; } = 0.05f;

        [field: SerializeField, Range(float.Epsilon, 180)] public float AngularSpeed { get; private set; } = 10f;

        [field: SerializeField, Range(float.Epsilon, 1)] public float AngularThreshold { get; private set; } = 0.05f;

        public bool MoveTowardsTarget(Vector3 target) 
        {
            float speed = Time.deltaTime * Speed;
            m_Rigidbody.MovePosition(Vector3.MoveTowards(LocomotionRoot.position, target, speed));

            return Vector3.Distance(LocomotionRoot.position, target) <= PositionThreshold;
        }

        public bool RotateTowardsDirection(Vector3 direction)
        {
            Quaternion desired = Quaternion.LookRotation(direction);
            float speed = Time.deltaTime * AngularSpeed;
            m_Rigidbody.MoveRotation(Quaternion.RotateTowards(RotationPivot.rotation, desired, speed));

            return Quaternion.Angle(RotationPivot.rotation, desired) <= AngularThreshold;
        }
    }
}