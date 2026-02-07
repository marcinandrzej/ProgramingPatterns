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

        //TO DO MOVE STATE MACHINE BUILDING WITH PARAMETRIZATION TO SEPARATE FACTORY (ObserverFactory, RunnerFactory ...)
        private StateMachine<AgentState, Agent> BuildObserver()
        {
            AgentState seekState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) =>
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new SeekCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .WithOnExit((next, agent) =>
                {
                    if (agent.CommandExecutor.IsCommandExecuting)
                        agent.CommandExecutor.CancelCurrentCommandExecution();
                })
                .Build();

            AgentState observeState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) => 
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new LookAtCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .WithOnExit((next, agent) =>
                {
                    if (agent.CommandExecutor.IsCommandExecuting)
                        agent.CommandExecutor.CancelCurrentCommandExecution();
                })
                .Build();

            Transition<AgentState, Agent> seekToObserve = new Transition<AgentState, Agent>(observeState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> observeToSeek = new Transition<AgentState, Agent>(seekState, agent => { return agent.DetectionModule.Target == null; }, 0);

            seekState.RegisterTransition(seekToObserve);
            observeState.RegisterTransition(observeToSeek);

            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(observeState);

            return machine;
        }

        private StateMachine<AgentState, Agent> BuildFollower() 
        {
            AgentState seekState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) =>
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new SeekCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .WithOnExit((next, agent) =>
                {
                    if (agent.CommandExecutor.IsCommandExecuting)
                        agent.CommandExecutor.CancelCurrentCommandExecution();
                })
                .Build();

            AgentState followState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) =>
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new FollowCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .WithOnExit((next, agent) =>
                {
                    if (agent.CommandExecutor.IsCommandExecuting)
                        agent.CommandExecutor.CancelCurrentCommandExecution();
                })
                .Build();

            Transition<AgentState, Agent> seekToFollow = new Transition<AgentState, Agent>(followState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> followToSeek = new Transition<AgentState, Agent>(seekState, agent => { return agent.DetectionModule.Target == null; }, 0);

            seekState.RegisterTransition(seekToFollow);
            followState.RegisterTransition(followToSeek);

            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(followState);

            return machine;
        }

        private StateMachine<AgentState, Agent> BuildRunner() 
        {
            AgentState seekState = new AgentStateBuilder()
                 .WithOnEnter((previous, agent) =>
                 {
                     List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new SeekCommand() };
                     agent.CommandExecutor.ExecuteCommand(commands);
                 })
                 .WithOnExit((next, agent) =>
                 {
                     if (agent.CommandExecutor.IsCommandExecuting)
                         agent.CommandExecutor.CancelCurrentCommandExecution();
                 })
                 .Build();

            AgentState runState = new AgentStateBuilder()
                .WithOnEnter((previous, agent) =>
                {
                    List<ICommand<Agent>> commands = new List<ICommand<Agent>>() { new EscapeCommand() };
                    agent.CommandExecutor.ExecuteCommand(commands);
                })
                .WithOnExit((next, agent) =>
                {
                    if(agent.CommandExecutor.IsCommandExecuting)
                        agent.CommandExecutor.CancelCurrentCommandExecution();
                })
                .Build();

            Transition<AgentState, Agent> seekToRun = new Transition<AgentState, Agent>(runState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> runToSeek = new Transition<AgentState, Agent>(seekState, agent => { return !agent.CommandExecutor.IsCommandExecuting; }, 0);

            seekState.RegisterTransition(seekToRun);
            runState.RegisterTransition(runToSeek);

            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(runState);

            return machine;
        }
    }
}
