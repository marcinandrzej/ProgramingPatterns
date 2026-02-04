using Runtime.Agent;
using Runtime.State.Agent;
using Runtime.State.Base;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Factory
{
    [CreateAssetMenu(fileName = "AgentStateMachineFactory", menuName = "Custom/Agent/Factory/StateMachine")]
    public class AgentStateMachineFactory : ScriptableObject
    {
        private const string SeekTarget = "SeekTarget";

        public StateMachine<AgentState, AgentData> GetStateMachine(EAgentType agentType) 
        {
            switch (agentType)
            {
                case EAgentType.Observer:
                    return BuildObserver();
                case EAgentType.Runner:
                    return BuildRunner();
                case EAgentType.Follower:
                    return BuildFollower();
                default:
                    return null;
            }
        }

        private StateMachine<AgentState, AgentData> BuildObserver()
        {
            AgentState seekState = new AgentStateBuilder()
                .WithOnUpdate(agent =>
                {
                    if (!agent.AgentRuntimeData.TryGet(SeekTarget, out Vector3 target) ||
                        IsTargetAimed(agent.LocomotionRoot, target, agent.AngularDotTreshold)) 
                    {
                        target = GetRandomSeekTarget(agent.LocomotionRoot);
                        agent.AgentRuntimeData.Set(SeekTarget, target);
                    }
                    
                    UpdateTargetAiming(agent.LocomotionRoot, target, agent.AngularSpeed);
                })
                .Build();

            AgentState observeState = new AgentStateBuilder()
                .WithOnUpdate(agent =>
                {
                    UpdateTargetAiming(agent.LocomotionRoot, agent.Target.position, agent.AngularSpeed);
                })
                .Build();

            Transition<AgentState, AgentData> seekToObserve = new Transition<AgentState, AgentData>(observeState, agent => { return agent.Target != null; }, 0);
            Transition<AgentState, AgentData> observeToSeek = new Transition<AgentState, AgentData>(seekState, agent => { return agent.Target == null; }, 0);

            seekState.RegisterTransition(seekToObserve);
            observeState.RegisterTransition(observeToSeek);

            StateMachine<AgentState, AgentData> machine = new StateMachine<AgentState, AgentData>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(observeState);

            return machine;
        }

        private StateMachine<AgentState, AgentData> BuildRunner() 
        {
            StateMachine<AgentState, AgentData> machine = new StateMachine<AgentState, AgentData>();

            //TO DO

            return machine;
        }

        private StateMachine<AgentState, AgentData> BuildFollower() 
        {
            StateMachine<AgentState, AgentData> machine = new StateMachine<AgentState, AgentData>();

            //TO DO

            return machine;
        }

        private Vector3 GetRandomSeekTarget(Transform pivot) 
        {
            int right = Random.value < 0.5f ? -1 : 1;
            Vector3 position = pivot.position + (right * pivot.right);
            
            return position;
        }

        private bool IsTargetAimed(Transform pivot, Vector3 target, float treshold)
        {
            Vector3 current = pivot.forward;
            Vector3 desired = (target - pivot.position).normalized;
            float dot = Vector3.Dot(desired, current);

            return dot > treshold;
        }

        private void UpdateTargetAiming(Transform pivot, Vector3 target, float rotationSpeed)
        {
            Vector3 desiredForward = (target - pivot.position).normalized;
            Quaternion current = pivot.rotation;
            Quaternion desired = Quaternion.LookRotation(desiredForward);
            float speed = Time.deltaTime * rotationSpeed;
            pivot.rotation = Quaternion.RotateTowards(current, desired, speed);
        }
    }
}
