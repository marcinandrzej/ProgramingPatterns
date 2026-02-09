using Runtime.Command.AgentCommands;
using Runtime.Command.Base;
using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Simple implementation of an Agent.
    /// Composes agent behaviour by wiring together modular subsystems:
    /// - Locomotion module (movement and rotation)
    /// - Detection module (target sensing)
    /// - Command executor (action sequencing)
    /// This class acts primarily as a composition root, exposing module implementations to the base Agent abstraction.
    /// </summary>
    public class AgentSimple : Agent
    {
        /// <summary>
        /// Handles movement and rotation logic for this agent.
        /// </summary>
        [SerializeField] private AgentLocomotionModuleSimple locomotionModule = null;

        /// <summary>
        /// Responsible for detecting and selecting targets in the environment.
        /// </summary>
        [SerializeField] private AgentDetectionModuleSimple detectionModule = null;

        /// <summary>
        /// Executes command chains controlling agent behaviour.
        /// </summary>
        [SerializeField] private AgentCommandExecutor commandExecutor = null;

        /// <summary>
        /// Provides command execution functionality to the base Agent system.
        /// </summary>
        public override ICommandExecutor<Agent> CommandExecutor => commandExecutor;

        /// <summary>
        /// Provides locomotion implementation used by behaviours and commands.
        /// </summary>
        public override IAgentLocomotionModule LocomotionModule => locomotionModule;

        /// <summary>
        /// Provides detection implementation used by decision-making systems.
        /// </summary>
        public override IAgentDetectionModule DetectionModule => detectionModule;
    }
}
