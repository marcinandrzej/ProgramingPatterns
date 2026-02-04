using System.Collections.Generic;
using System.Linq;

namespace Runtime.State.Base
{
    public class StateMachine<StateT, AgentT> where StateT : StateBase<StateT, AgentT>
    {
        private HashSet<StateT> states = null;
        private StateT targetState = null;
        private StateT currentState = null;
        private StateT startingState = null;

        public StateMachine()
        {
            states = new HashSet<StateT>();
            targetState = null;
        }

        public void Start(AgentT agent)
        {
            targetState = startingState != null ? startingState : states.ElementAt(0);
            ChangeState(targetState, agent);
        }

        public void Update(AgentT agent)
        {
            targetState = currentState.CheckTransitions(agent);

            if (targetState != null)
                ChangeState(targetState, agent);
            else
                currentState.Update(agent);
        }

        public void RegisterState(StateT state, bool isStarting = false)
        {
            if (states.Add(state) && states.Count == 0 && isStarting)
                startingState = state;
        }

        private void ChangeState(StateT nextState, AgentT agent)
        {
            currentState?.Exit(nextState, agent);
            nextState.Enter(currentState, agent);
            currentState = nextState;
        }
    }
}