using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace State.Generic
{
    public class StateMachineT<T> where T : StateT<T>
    {
        private HashSet<T> states = null;
        private T targetState = null;
        private T currentState = null;

        public StateMachineT()
        {
            states = new HashSet<T>();
            targetState = null;
        }

        public void Initialize()
        {
            foreach (T state in states)
                state.Initialize();
        }

        public void Start(T state = null)
        {
            targetState = state != null ? state : targetState != null ? targetState : states.Count > 0 ? states.ElementAt(0) : null;

            if (targetState != null)
                ChangeState(targetState);
            else
                Debug.LogWarning("State machine has no states");
        }

        public void Update()
        {
            if (currentState == null)
                return;

            currentState.Update();
            targetState = currentState.CheckTransitions();

            if (targetState != null)
                ChangeState(targetState);
        }

        public void Deinitialize()
        {
            foreach (T state in states)
                state.Deinitialize();

            states.Clear();
            states = null;
            targetState = null;
            currentState = null;
        }

        public void RegisterState(T state)
        {
            if (states.Add(state) && states.Count == 0)
                targetState = state;
        }

        private void ChangeState(T nextState)
        {
            currentState?.Exit(nextState);
            nextState.Enter(currentState);
            currentState = nextState;
        }
    }
}