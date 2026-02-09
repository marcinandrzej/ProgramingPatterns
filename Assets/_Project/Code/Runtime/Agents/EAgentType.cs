namespace Runtime.Agents
{
    /// <summary>
    /// Defines available agent archetypes used to configure behaviour.
    /// Used by factories to select appropriate state machines.
    /// </summary>
    public enum EAgentType
    {
        /// <summary>
        /// Passive agent focused on observation or scanning behaviour.
        /// </summary>
        Observer,

        /// <summary>
        /// Agent that runs from a target entity.
        /// </summary>
        Runner,

        /// <summary>
        /// Agent that follows or tracks a target entity.
        /// </summary>
        Follower
    }
}
