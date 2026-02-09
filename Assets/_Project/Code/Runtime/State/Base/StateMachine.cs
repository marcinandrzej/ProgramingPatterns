using System.Collections.Generic;
using System.Linq;

namespace Runtime.State.Base
{
    /// <summary>
    /// Generic finite state machine implementation.
    /// Responsible for:
    /// - managing current state
    /// - evaluating transitions
    /// - invoking lifecycle methods
    /// </summary>
    public class StateMachine<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        // Registered states.
        private HashSet<StateT> states = null;

        // Current and next state references.
        private StateT targetState = null;
        private StateT currentState = null;
        private StateT startingState = null;

        public StateMachine()
        {
            states = new HashSet<StateT>();
            targetState = null;
        }

        /// <summary>
        /// Initializes state machine and enters starting state.
        /// </summary>
        public void Start(AgentT agent)
        {
            targetState = startingState != null ? startingState : states.ElementAt(0);
            ChangeState(targetState, agent);
        }

        /// <summary>
        /// Updates current state and evaluates transitions.
        /// </summary>
        public void Update(AgentT agent)
        {
            targetState = currentState.CheckTransitions(agent);

            if (targetState != null)
                ChangeState(targetState, agent);
            else
                currentState.Update(agent);
        }

        /// <summary>
        /// Registers state with optional starting flag.
        /// </summary>
        public void RegisterState(StateT state, bool isStarting = false)
        {
            if (states.Add(state) && states.Count == 0 && isStarting)
                startingState = state;
        }

        /// <summary>
        /// Handles exit and enter lifecycle during state change.
        /// </summary>
        private void ChangeState(StateT nextState, AgentT agent)
        {
            currentState?.Exit(nextState, agent);
            nextState.Enter(currentState, agent);
            currentState = nextState;
        }
    }
}
