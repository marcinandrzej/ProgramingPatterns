using System;

namespace Runtime.State.Base
{
    /// <summary>
    /// Represents a directed transition between two states.
    /// A transition contains:
    /// - target state
    /// - condition delegate determining when transition is valid
    /// - priority used when multiple transitions are available
    /// Higher priority transitions are evaluated first.
    /// </summary>
    public class Transition<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        // Condition evaluated each update to determine if transition should occur.
        private Func<AgentT, bool> Condition;

        /// <summary>
        /// State to switch to when condition evaluates to true.
        /// </summary>
        public StateT ToState { get; private set; }

        /// <summary>
        /// Transition priority. Higher values are evaluated first.
        /// </summary>
        public int Priority { get; private set; }

        public Transition(StateT toState, Func<AgentT, bool> condition, int priority)
        {
            Priority = priority;
            ToState = toState;
            Condition = condition;
        }

        /// <summary>
        /// Evaluates transition condition for given agent.
        /// </summary>
        public bool CheckCondition(AgentT obj) => Condition.Invoke(obj);
    }
}
