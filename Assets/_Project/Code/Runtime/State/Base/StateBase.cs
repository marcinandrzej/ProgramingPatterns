using System;
using System.Collections.Generic;

namespace Runtime.State.Base
{
    /// <summary>
    /// Base abstraction for a state within a generic state machine.
    /// Defines lifecycle callbacks:
    /// - Enter
    /// - Update
    /// - Exit
    /// Supports prioritized transitions evaluated each update.
    /// </summary>
    public abstract class StateBase<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        // Lifecycle callbacks configured via builder.
        private event Action<StateT, AgentT> OnEnter = null;
        private event Action<StateT, AgentT> OnExit = null;
        private event Action<AgentT> OnUpdate = null;

        // List of transitions originating from this state.
        private List<Transition<StateT, AgentT>> transitions = null;

        public StateBase() 
        {
            // Default empty delegates to avoid null checks.
            OnEnter = (StateT, ObjectT) => { };
            OnExit = (StateT, ObjectT) => { };
            OnUpdate = (ObjectT) => { };
            transitions = new List<Transition<StateT, AgentT>>();
        }

        /// <summary>
        /// Registers a transition and sorts transitions by priority.
        /// Higher priority transitions are evaluated first.
        /// </summary>
        public void RegisterTransition(Transition<StateT, AgentT> transition)
        {
            transitions.Add(transition);
            transitions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        // Internal lifecycle setters used by builder.
        internal void SetOnEnter(Action<StateT, AgentT> action) => OnEnter = action;

        internal void SetOnExit(Action<StateT, AgentT> action) => OnExit = action;

        internal void SetOnUpdate(Action<AgentT> action) => OnUpdate = action;

        // Lifecycle invocations handled by state machine.
        internal void Enter(StateT previousState, AgentT obj) => OnEnter.Invoke(previousState, obj);

        internal void Exit(StateT nextState, AgentT obj) => OnExit.Invoke(nextState, obj);

        internal void Update(AgentT obj) => OnUpdate.Invoke(obj);

        /// <summary>
        /// Evaluates transitions in priority order.
        /// Returns first valid target state or null if no transition applies.
        /// </summary>
        internal StateT CheckTransitions(AgentT obj)
        {
            for (int i = 0; i < transitions.Count; i++)
                if (transitions[i].CheckCondition(obj))
                    return transitions[i].ToState;

            return null;
        }
    }
}
