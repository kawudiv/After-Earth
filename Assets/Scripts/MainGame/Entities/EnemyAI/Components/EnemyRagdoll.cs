using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemyRagdoll : MonoBehaviour
    {
        Rigidbody[] rigidbodies;
        Animator animator;

        void Start()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            animator = GetComponent<Animator>();

            DeactivateRagdoll();
        }

        public void DeactivateRagdoll()
        {
            foreach (var Rigidbody in rigidbodies)
            {
                Rigidbody.isKinematic = true;
            }
            animator.enabled = true;
        }

        public void ActivateRagdoll()
        {
            foreach (var Rigidbody in rigidbodies)
            {
                Rigidbody.isKinematic = false;
            }
            animator.enabled = false;
        }
    }
}
