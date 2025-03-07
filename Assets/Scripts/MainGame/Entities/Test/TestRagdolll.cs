using UnityEngine;

namespace EnemyAi.Test
{
    public class TestRagdoll : MonoBehaviour
    {
        private Rigidbody[] rigidbodies;
        private Animator animator;

        void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            animator = GetComponent<Animator>();
            DeactivateRagdoll();
        }

        public void DeactivateRagdoll()
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            animator.enabled = true;
        }

        public void ActivateRagdoll()
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            animator.enabled = false;
        }

        public void TriggerRagdoll()
        {
            ActivateRagdoll();
            Debug.Log("💀 Enemy has entered ragdoll mode!");
        }

        public void ApplyForceToRagdoll(Vector3 hitPoint, Vector3 force)
        {
            Rigidbody closestRb = null;
            float closestDistance = Mathf.Infinity;

            // Find the closest Rigidbody to the hit point
            foreach (var rb in rigidbodies)
            {
                float distance = Vector3.Distance(hitPoint, rb.worldCenterOfMass);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRb = rb;
                }
            }

            // Apply force to the closest limb
            if (closestRb != null)
            {
                closestRb.AddForce(force, ForceMode.Impulse);
                Debug.Log("💥 Ragdoll hit with force: " + force);
            }
        }
    }
}
