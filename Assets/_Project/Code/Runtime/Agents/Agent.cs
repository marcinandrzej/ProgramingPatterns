using Runtime.Command.Base;
using Runtime.Factory.AgentFactories;
using Runtime.State.AgentStates;
using Runtime.State.Base;
using UnityEngine;

namespace Runtime.Agents
{
    public abstract class Agent : MonoBehaviour
    {
        [SerializeField] private EAgentType agentType = EAgentType.Observer;
        [SerializeField] private AgentStateMachineFactory stateMachineFactory = null;

        private StateMachine<AgentState, Agent> stateMachine = null;

        private void Awake() => stateMachine = stateMachineFactory.GetStateMachine(agentType);

        private void Start() => stateMachine.Start(this);

        private void Update() => stateMachine.Update(this);

        public abstract ICommandExecutor<Agent> CommandExecutor { get; }

        public abstract IAgentLocomotionModule LocomotionModule { get; }

        //TO DO MOVE TO DEDICATED CONTROLLERS
        //[field: SerializeField] public float Speed { get; private set; } = 1f;

        //[field: SerializeField, Range(float.Epsilon, 180)] public float AngularSpeed { get; private set; } = 10f;

        //[field: SerializeField, Range(float.Epsilon, 1)] public float AngularDotTreshold { get; private set; } = 0.95f;

        //[field: SerializeField] public Transform LocomotionRoot { get; private set; }

        //[field: SerializeField] public Transform Target { get; private set; }

        //private Vector3 GetRandomSeekTarget(Transform pivot)
        //{
        //    int right = Random.value < 0.5f ? -1 : 1;
        //    Vector3 position = pivot.position + (right * pivot.right);

        //    return position;
        //}

        //private bool IsTargetAimed(Transform pivot, Vector3 target, float treshold)
        //{
        //    Vector3 current = pivot.forward;
        //    Vector3 desired = (target - pivot.position).normalized;
        //    float dot = Vector3.Dot(desired, current);

        //    return dot > treshold;
        //}

        //private void UpdateTargetAiming(Transform pivot, Vector3 target, float rotationSpeed)
        //{
        //    Vector3 desiredForward = (target - pivot.position).normalized;
        //    Quaternion current = pivot.rotation;
        //    Quaternion desired = Quaternion.LookRotation(desiredForward);
        //    float speed = Time.deltaTime * rotationSpeed;
        //    pivot.rotation = Quaternion.RotateTowards(current, desired, speed);
        //}
    }
}
