using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Defines the contract for agent detection systems.
    /// Detection modules are responsible for sensing the environment and providing a currently selected target for the agent.
    /// Assumptions:
    /// - Target may be null if no valid target is detected.
    /// - Returned transform should represent the root object of the detected entity.
    /// - Implementations are responsible for keeping the target reference up-to-date.
    /// </summary>
    public interface IAgentDetectionModule
    {
        /// <summary>
        /// Currently selected target detected by the module.
        /// Returns null when no valid target is available.
        /// </summary>
        public Transform Target { get; }
    }
}
