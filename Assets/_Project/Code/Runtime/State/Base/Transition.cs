using System;

namespace Runtime.State.Base
{
    public class Transition<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        private event Func<AgentT, bool> Condition;

        public StateT ToState { get; private set; }

        public int Priority { get; private set; }

        public Transition(StateT toState, Func<AgentT, bool> condition, int priority)
        {
            Priority = priority;
            ToState = toState;
            Condition = condition;
        }

        public bool CheckCondition(AgentT obj) => Condition.Invoke(obj);
    }
}