using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Agents
{
    public class AgentDetectionModuleSimple : MonoBehaviour, IAgentDetectionModule
    {
        private HashSet<Transform> targets = new HashSet<Transform>();

        public Transform Target { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            targets.Add(other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform);
            
            if (Target == null)
                SelectNewTarget();
        }

        private void OnTriggerExit(Collider other)
        {
            targets.Remove(other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform);

            if (!targets.Contains(Target))
                SelectNewTarget();
        }

        private void SelectNewTarget() => Target = targets.Where(x => x != null).OrderBy(x => Random.value).FirstOrDefault();
    }
}