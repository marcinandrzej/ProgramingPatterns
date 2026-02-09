using System;

namespace Runtime.State.Base
{
    /// <summary>
    /// Fluent builder used to configure state lifecycle callbacks.
    /// Uses CRTP (Curiously Recurring Template Pattern) pattern to preserve fluent API typing.
    /// </summary>
    public abstract class StateBuilderBase<StateT, AgentT, BuilderT> where StateT : StateBase<StateT, AgentT> where BuilderT : StateBuilderBase<StateT, AgentT, BuilderT>
    {
        // State instance being configured.
        protected StateT state = null;

        // Creates state instance dynamically.
        public StateBuilderBase() => state = Activator.CreateInstance<StateT>();

        /// <summary>
        /// Defines enter behaviour.
        /// </summary>
        public BuilderT WithOnEnter(Action<StateT, AgentT> onEnter) 
        {
            state.SetOnEnter(onEnter);

            return this as BuilderT;
        }

        /// <summary>
        /// Defines exit behaviour.
        /// </summary>
        public BuilderT WithOnExit(Action<StateT, AgentT> onExit)
        {
            state.SetOnExit(onExit);

            return this as BuilderT;
        }

        /// <summary>
        /// Defines update behaviour.
        /// </summary>
        public BuilderT WithOnUpdate(Action<AgentT> onUpdate)
        {
            state.SetOnUpdate(onUpdate);

            return this as BuilderT;
        }

        /// <summary>
        /// Returns fully configured state instance.
        /// </summary>
        public StateT Build() => state;
    }
}
