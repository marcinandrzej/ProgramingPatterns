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

        public abstract IAgentDetectionModule DetectionModule { get; }

    }
}
