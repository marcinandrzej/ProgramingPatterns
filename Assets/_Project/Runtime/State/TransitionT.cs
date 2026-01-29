using System;

namespace State.Generic
{
    public class TransitionT<T> where T : StateT<T>
    {
        private event Func<bool> Condition;

        public T ToState { get; private set; }

        public int Priority { get; private set; }

        public TransitionT(T toState, Func<bool> condition, int priority)
        {
            Priority = priority;
            ToState = toState;
            Condition = condition;
        }

        public bool CheckCondition() => Condition.Invoke();
    }
}