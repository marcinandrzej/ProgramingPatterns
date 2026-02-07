using UnityEngine;

namespace Runtime.Agents
{
    public interface IAgentDetectionModule
    {
        public Transform Target { get; }
    }
}