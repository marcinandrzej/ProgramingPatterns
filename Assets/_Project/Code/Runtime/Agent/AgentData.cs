using System;
using UnityEngine;

namespace Runtime.Agent
{
    [Serializable]
    public class AgentData
    {
        // RuntimeData
        [field: SerializeField] public AgentBlackboard AgentRuntimeData { get; private set; } = new AgentBlackboard();

        //Persistent data
        [field: SerializeField] public float Speed { get; private set; } = 1f;

        [field: SerializeField, Range(float.Epsilon, 180)] public float AngularSpeed { get; private set; } = 10f;

        [field: SerializeField, Range(float.Epsilon, 1)] public float AngularDotTreshold { get; private set; } = 0.95f;

        [field: SerializeField] public Transform LocomotionRoot { get; private set; }
        //TO DO Target provider
        [field: SerializeField] public Transform Target { get; private set; }
    }
}