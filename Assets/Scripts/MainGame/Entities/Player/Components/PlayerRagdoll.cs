using UnityEngine;

namespace Player.Components
{
    public class PlayerRagdoll : MonoBehaviour
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
            Debug.Log("Activating Ragdoll");
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
            Debug.Log("💀 Player has entered ragdoll mode!");
        }

        public void ApplyForceToRagdoll(Vector3 hitPoint, Vector3 force)
        {
            Rigidbody closestRb = null;
            float closestDistance = Mathf.Infinity;

            foreach (var rb in rigidbodies)
            {
                float distance = Vector3.Distance(hitPoint, rb.worldCenterOfMass);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRb = rb;
                }
            }

            if (closestRb != null)
            {
                closestRb.AddForce(force, ForceMode.Impulse);
                Debug.Log("💥 Player ragdoll hit with force: " + force);
            }
        }
    }
} 