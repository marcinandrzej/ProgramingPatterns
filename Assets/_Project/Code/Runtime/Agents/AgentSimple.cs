using Runtime.Command.AgentCommands;
using Runtime.Command.Base;
using UnityEngine;

namespace Runtime.Agents
{
    public class AgentSimple : Agent
    {
        [SerializeField] private AgentLocomotionModuleSimple locomotionModule = null;
        [SerializeField] private AgentDetectionModuleSimple detectionModule = null;
        [SerializeField] private AgentCommandExecutor commandExecutor = null;

        public override ICommandExecutor<Agent> CommandExecutor => commandExecutor;

        public override IAgentLocomotionModule LocomotionModule => locomotionModule;

        public override IAgentDetectionModule DetectionModule => detectionModule;
    }
}
