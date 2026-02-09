using Runtime.Agents;
using Runtime.Command.AgentCommands;
using Runtime.Command.Base;
using Runtime.State.AgentStates;
using Runtime.State.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Factory.AgentFactories
{
    /// <summary>
    /// Factory responsible for creating Agent-specific state machines.
    /// Supports multiple agent archetypes (Observer, Runner, Follower).
    /// Each state machine is constructed with:
    /// - Typed states (AgentState)
    /// - Transitions based on agent detection or command execution
    /// - State entry/exit commands using ICommandExecutor
    /// </summary>
    [CreateAssetMenu(fileName = "AgentStateMachineFactory", menuName = "Custom/Agent/Factory/StateMachine")]
    public class AgentStateMachineFactory : ScriptableObject
    {
        /// <summary>
        /// Returns a fully configured state machine for a given agent type.
        /// </summary>
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

        /// <summary>
        /// Builds state machine for Observer agents.
        /// Contains:
        /// - Seek state: agent rotates/scans environment until a target is detected
        /// - Observe state: agent looks at target
        /// Transitions:
        /// - Seek -> Observe: target detected
        /// - Observe -> Seek: target lost
        /// </summary>
        private StateMachine<AgentState, Agent> BuildObserver()
        {
            // Seek state: rotate/look around until a target appears
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

            // Observe state: look at detected target
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

            // Transition definitions
            Transition<AgentState, Agent> seekToObserve = new Transition<AgentState, Agent>(observeState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> observeToSeek = new Transition<AgentState, Agent>(seekState, agent => { return agent.DetectionModule.Target == null; }, 0);

            seekState.RegisterTransition(seekToObserve);
            observeState.RegisterTransition(observeToSeek);

            // Assemble state machine
            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(observeState);

            return machine;
        }

        /// <summary>
        /// Builds state machine for Follower agents.
        /// Contains:
        /// - Seek state: locate target
        /// - Follow state: move towards detected target
        /// Transitions:
        /// - Seek -> Follow: target detected
        /// - Follow -> Seek: target lost
        /// </summary>
        private StateMachine<AgentState, Agent> BuildFollower() 
        {
            // Seek state: locate target
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

            // Follow state: move toward target
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

            // Define transitions
            Transition<AgentState, Agent> seekToFollow = new Transition<AgentState, Agent>(followState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> followToSeek = new Transition<AgentState, Agent>(seekState, agent => { return agent.DetectionModule.Target == null; }, 0);

            seekState.RegisterTransition(seekToFollow);
            followState.RegisterTransition(followToSeek);

            // Assemble the state machine
            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(followState);

            return machine;
        }

        /// <summary>
        /// Constructs Runner agent state machine:
        /// - Seek: locate threat
        /// - Run: escape from threat
        /// Transitions:
        /// - Seek -> Run: target detected
        /// - Run -> Seek: escape command completed
        /// </summary>
        private StateMachine<AgentState, Agent> BuildRunner() 
        {
            // Seek state: locate threat
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

            // Run state: escape
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

            // Define transitions
            Transition<AgentState, Agent> seekToRun = new Transition<AgentState, Agent>(runState, agent => { return agent.DetectionModule.Target != null; }, 0);
            Transition<AgentState, Agent> runToSeek = new Transition<AgentState, Agent>(seekState, agent => { return !agent.CommandExecutor.IsCommandExecuting; }, 0);

            seekState.RegisterTransition(seekToRun);
            runState.RegisterTransition(runToSeek);

            // Assemble the state machine
            StateMachine<AgentState, Agent> machine = new StateMachine<AgentState, Agent>();
            machine.RegisterState(seekState, true);
            machine.RegisterState(runState);

            return machine;
        }
    }
}
