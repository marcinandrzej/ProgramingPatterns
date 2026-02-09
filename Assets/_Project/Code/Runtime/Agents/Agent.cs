using Runtime.Command.Base;
using Runtime.Factory.AgentFactories;
using Runtime.State.AgentStates;
using Runtime.State.Base;
using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Base abstraction for all agents in the simulation.
    /// Owns and updates agent state machine
    /// Exposes modular subsystems (locomotion, detection, command execution)
    /// Child classes provide implementations for modules while core behaviour is centralized here.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Agent : MonoBehaviour
    {
        [SerializeField] private EAgentType agentType = EAgentType.Observer;

        // Factory used to construct state machine depending on agent type.
        // This prevents tight coupling between Agent and concrete state implementations.
        [SerializeField] private AgentStateMachineFactory stateMachineFactory = null;

        // Runtime state machine controlling agent behaviour.
        private StateMachine<AgentState, Agent> stateMachine = null;

        /// <summary>
        /// Initializes state machine during Awake to ensure availability before Start() execution.
        /// </summary>
        private void Awake() => stateMachine = stateMachineFactory.GetStateMachine(agentType);

        /// <summary>
        /// Starts agent behaviour.
        /// </summary>
        private void Start() => stateMachine.Start(this);

        /// <summary>
        /// Delegates update logic to current state.
        /// </summary>
        private void Update() => stateMachine.Update(this);

        /// <summary>
        /// Executes command chains controlling agent actions.
        /// </summary>
        public abstract ICommandExecutor<Agent> CommandExecutor { get; }

        /// <summary>
        /// Handles movement and rotation.
        /// </summary>
        public abstract IAgentLocomotionModule LocomotionModule { get; }

        /// <summary>
        /// Handles environment sensing and detection logic.
        /// </summary>
        public abstract IAgentDetectionModule DetectionModule { get; }

    }
}
