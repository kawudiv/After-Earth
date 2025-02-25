using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Debug.Log($"[PlayerAnimation] Awake called on {gameObject.name}", this);
        }

        public void UpdateSpeed(float targetSpeed)
        {
            // Get the current speed parameter from the animator.
            float currentAnimSpeed = animator.GetFloat("Speed");
            // Smoothly interpolate toward the targetSpeed using Lerp.
            float newAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, 10f * Time.deltaTime);
            animator.SetFloat("Speed", newAnimSpeed);
            Debug.Log(
                $"[PlayerAnimation] Speed updated: {currentAnimSpeed:F2} -> {newAnimSpeed:F2} on {gameObject.name}",
                this
            );
        }

        public void SetTrigger(string parameter)
        {
            animator.SetTrigger(parameter);
            Debug.Log($"[PlayerAnimation] SetTrigger: {parameter} on {gameObject.name}", this);
        }

        public void PlayHitReaction()
        {
            animator.SetTrigger("Hit");
            Debug.Log($"[PlayerAnimation] PlayHitReaction triggered on {gameObject.name}", this);
        }

        public void PlayDeath()
        {
            animator.SetTrigger("Death");
            Debug.Log($"[PlayerAnimation] PlayDeath triggered on {gameObject.name}", this);
        }

        public void ResetTrigger(string triggerName)
        {
            animator.ResetTrigger(triggerName);
            Debug.Log($"[PlayerAnimation] ResetTrigger: {triggerName} on {gameObject.name}", this);
        }
    }
}
