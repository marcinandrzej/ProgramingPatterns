using Runtime.Agents;
using Runtime.Command.AgentCommands;
using Runtime.Command.Base;
using Runtime.State.AgentStates;
using Runtime.State.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Factory.AgentFactories
{
    [CreateAssetMenu(fileName = "AgentStateMachineFactory", menuName = "Custom/Agent/Factory/StateMachine")]
    public class AgentStateMachineFactory : ScriptableObject
    {
        public StateMachine<AgentState, Agent> GetStateMachine(EAgentType agentType) 
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

        private StateMachine<AgentState, Agent> BuildObserver()
        {
            AgentState seekState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) =>
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new SeekCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .Build();

            AgentState observeState = new AgentStateBuilder().Build();

            Transition<AgentState, Agent> seekToObserve = new Transition<AgentState, Agent>(observeState, agent => { return false; }, 0);
            Transition<AgentState, Agent> observeToSeek = new Transition<AgentState, Agent>(seekState, agent => { return false; }, 0);

            seekState.RegisterTransition(seekToObserve);
            observeState.RegisterTransition(observeToSeek);

            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(observeState);

            return machine;
        }

        private StateMachine<AgentState, Agent> BuildRunner() 
        {
            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();

            //TO DO

            return machine;
        }

        private StateMachine<AgentState, Agent> BuildFollower() 
        {
            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();

            //TO DO

            return machine;
        }
    }
}
