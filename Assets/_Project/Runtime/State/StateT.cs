using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace State.Generic
{
    public abstract class StateT<T> where T : StateT<T>
    {
        private event Action<T> OnEnter = null;
        private event Action<T> OnExit = null;
        private event Action OnUpdate = null;
        private event Action OnInitialize = null;
        private event Action OnDeinitialize = null;

        private List<TransitionT<T>> transitions = null;

        public StateT(Action<T> onEnter, Action<T> onExit, Action onUpdate, Action onInitialize, Action onDeinitialize)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            OnUpdate = onUpdate;
            OnInitialize = onInitialize;
            OnDeinitialize = onDeinitialize;
            transitions = new List<TransitionT<T>>();
        }

        public void Enter(T previousState) => OnEnter.Invoke(previousState);

        public void Exit(T nextState) => OnExit.Invoke(nextState);

        public void Update() => OnUpdate.Invoke();

        public void Initialize() => OnInitialize.Invoke();

        public void Deinitialize() => OnDeinitialize.Invoke();

        public void RegisterTransition(TransitionT<T> transition)
        {
            transitions.Add(transition);
            transitions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        public T CheckTransitions()
        {
            for (int i = 0; i < transitions.Count; i++)
                if (transitions[i].CheckCondition())
                    return transitions[i].ToState;

            return null;
        }
    }
}
