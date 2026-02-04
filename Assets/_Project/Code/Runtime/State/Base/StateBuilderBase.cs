using System;

namespace Runtime.State.Base
{
    public abstract class StateBuilderBase<StateT, AgentT, BuilderT> where StateT : StateBase<StateT, AgentT> where BuilderT : StateBuilderBase<StateT, AgentT, BuilderT>
    {
        protected StateT state = null;

        public StateBuilderBase() => state = Activator.CreateInstance<StateT>();

        public BuilderT WithOnEnter(Action<StateT, AgentT> onEnter) 
        {
            state.SetOnEnter(onEnter);

            return this as BuilderT;
        }

        public BuilderT WithOnExit(Action<StateT, AgentT> onExit)
        {
            state.SetOnExit(onExit);

            return this as BuilderT;
        }

        public BuilderT WithOnUpdate(Action<AgentT> onUpdate)
        {
            state.SetOnUpdate(onUpdate);

            return this as BuilderT;
        }

        public StateT Build() => state;
    }
}
