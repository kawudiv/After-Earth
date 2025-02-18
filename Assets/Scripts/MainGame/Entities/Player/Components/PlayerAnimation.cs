using Player.Base;
using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;
        private PlayerBase player;

        private float currentSpeed = 0f;
        private float speedTransitionDuration = 0.15f; // ⏩ Faster transition (Reduced from 0.3s)

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            player = GetComponent<PlayerBase>();

            Debug.Log($"[PlayerAnimation] Awake called on {gameObject.name}", this);
        }

        private void Update()
        {
            float targetSpeed = characterController.velocity.magnitude;

            // ✅ Speed up transition (faster reaction when starting to run)
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 10f * Time.deltaTime);
            animator.SetFloat("Speed", currentSpeed);

            Debug.Log($"[PlayerAnimation] Updating Speed: {currentSpeed:F2} on {gameObject.name}", this);
        }

        /// ✅ Set movement speed smoothly but faster
        public void SetMovementSpeed(float speed)
        {
            float oldSpeed = currentSpeed;

            // ✅ Use a faster transition (instant response when needed)
            if (speed > currentSpeed + 0.1f) 
            {
                currentSpeed = speed; // Snap to speed if increasing quickly
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, speed, 10f * Time.deltaTime);
            }

            animator.SetFloat("Speed", currentSpeed);
            Debug.Log($"[PlayerAnimation] Speed Changed: {oldSpeed:F2} → {currentSpeed:F2} on {gameObject.name}", this);
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
