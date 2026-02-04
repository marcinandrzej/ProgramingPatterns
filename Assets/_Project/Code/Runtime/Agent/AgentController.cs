using Runtime.Factory;
using Runtime.State.Agent;
using Runtime.State.Base;
using UnityEngine;

namespace Runtime.Agent
{
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private EAgentType agentType = EAgentType.Observer;
        [SerializeField] private AgentStateMachineFactory stateMachineFactory = null;

        [field: SerializeField] public AgentData AgentData{ get; private set; }

        private StateMachine<AgentState, AgentData> stateMachine = null;

        private void Awake()
        {
            stateMachine = stateMachineFactory.GetStateMachine(agentType);
        }

        private void Start()
        {
            stateMachine.Start(AgentData);
        }

        private void Update()
        {
            stateMachine.Update(AgentData);
        }
    }
}
