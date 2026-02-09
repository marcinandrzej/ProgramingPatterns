using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Agents
{
    /// <summary>
    /// Simple detection module based on trigger volumes.
    /// Maintains a dynamic set of nearby targets and selects one active target.
    /// Target selection is randomized whenever the current target becomes invalid.
    /// Requires collision matrix setup to prevent invalid detections.
    /// Other objects entering the trigger represent detectable targets.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AgentDetectionModuleSimple : MonoBehaviour, IAgentDetectionModule
    {
        // Stores targets being currently inside trigger volume.
        // HashSet is used to prevent duplicates and allow fast add/remove.
        private HashSet<Transform> targets = new HashSet<Transform>();

        /// <summary>
        /// Currently selected target from detected objects.
        /// May be null when no valid targets exist.
        /// </summary>
        public Transform Target { get; private set; }

        /// <summary>
        /// Adds new target when collider enters trigger.
        /// If the collider belongs to a rigidbody, its transform is used to avoid detecting individual collider parts separately.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            targets.Add(other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform);

            // If no target is currently selected, choose one immediately.
            if (Target == null)
                SelectNewTarget();
        }

        /// <summary>
        /// Removes target when collider exits trigger.
        /// If current target becomes invalid, select a new one.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            targets.Remove(other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform);

            // If current target is no longer present, choose another.
            if (!targets.Contains(Target))
                SelectNewTarget();
        }

        /// <summary>
        /// Selects a new target randomly from available detected objects.
        /// Filters out destroyed/null references before selection.
        /// </summary>
        private void SelectNewTarget() 
        {
            targets.RemoveWhere(t => t == null);

            if (targets.Count == 0)
            {
                Target = null;

                return;
            }

            int index = Random.Range(0, targets.Count);
            Target = targets.ElementAt(index);
        }
    }
}
