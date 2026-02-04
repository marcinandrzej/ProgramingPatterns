using System;
using System.Collections.Generic;

namespace Runtime.State.Base
{
    public abstract class StateBase<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        private event Action<StateT, AgentT> OnEnter = null;
        private event Action<StateT, AgentT> OnExit = null;
        private event Action<AgentT> OnUpdate = null;

        private List<Transition<StateT, AgentT>> transitions = null;

        public StateBase() 
        {
            OnEnter = (StateT, ObjectT) => { };
            OnExit = (StateT, ObjectT) => { };
            OnUpdate = (ObjectT) => { };
            transitions = new List<Transition<StateT, AgentT>>();
        }

        public void RegisterTransition(Transition<StateT, AgentT> transition)
        {
            transitions.Add(transition);
            transitions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        internal void SetOnEnter(Action<StateT, AgentT> action) => OnEnter = action;

        internal void SetOnExit(Action<StateT, AgentT> action) => OnExit = action;

        internal void SetOnUpdate(Action<AgentT> action) => OnUpdate = action;

        internal void Enter(StateT previousState, AgentT obj) => OnEnter.Invoke(previousState, obj);

        internal void Exit(StateT nextState, AgentT obj) => OnExit.Invoke(nextState, obj);

        internal void Update(AgentT obj) => OnUpdate.Invoke(obj);

        internal StateT CheckTransitions(AgentT obj)
        {
            for (int i = 0; i < transitions.Count; i++)
                if (transitions[i].CheckCondition(obj))
                    return transitions[i].ToState;

            return null;
        }
    }
}
