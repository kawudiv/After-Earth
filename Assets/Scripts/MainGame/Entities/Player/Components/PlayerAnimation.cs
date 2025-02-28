using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;

        private static readonly int SpeedParam = Animator.StringToHash("Speed");
        private static readonly int MeleeWeaponTypeParam = Animator.StringToHash("Melee");
        private static readonly int DrawMeleeParam = Animator.StringToHash("DrawMelee");
        private static readonly int SheathParam = Animator.StringToHash("Sheath");
        private static readonly int HitParam = Animator.StringToHash("Hit");
        private static readonly int DeathParam = Animator.StringToHash("Death");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Debug.Log($"[PlayerAnimation] Awake called on {gameObject.name}", this);
        }

        /// <summary>
        /// Updates movement speed in the Animator with smooth interpolation.
        /// </summary>
        public void UpdateSpeed(float targetSpeed)
        {
            float currentAnimSpeed = animator.GetFloat(SpeedParam);
            float newAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, 10f * Time.deltaTime);
            animator.SetFloat(SpeedParam, newAnimSpeed);
        }

        /// <summary>
        /// Sets an animation trigger.
        /// </summary>
        public void SetTrigger(string parameter)
        {
            ResetAllTriggers(); // ✅ Prevents animation conflicts
            animator.SetTrigger(parameter);
            Debug.Log($"[PlayerAnimation] SetTrigger: {parameter} on {gameObject.name}", this);
        }

        /// <summary>
        /// Sets the melee weapon type parameter in the animator.
        /// </summary>
        public void SetMeleeWeaponType(int weaponID)
        {
            animator.SetFloat(MeleeWeaponTypeParam, weaponID);
            Debug.Log($"[PlayerAnimation] Set Melee parameter to: {weaponID}");
        }

        /// <summary>
        /// Plays the correct animation for drawing or sheathing a melee weapon.
        /// </summary>
        public void SetDrawSheathAnimation(bool isDrawing)
        {
            ResetAllTriggers(); // ✅ Clears previous animation triggers

            if (isDrawing)
            {
                animator.SetTrigger(DrawMeleeParam);
                Debug.Log("[PlayerAnimation] Drawing melee weapon.");
            }
            else
            {
                animator.SetTrigger(SheathParam);
                Debug.Log("[PlayerAnimation] Sheathing melee weapon.");
            }
        }

        /// <summary>
        /// Plays the hit reaction animation.
        /// </summary>
        public void PlayHitReaction()
        {
            animator.SetTrigger(HitParam);
            Debug.Log($"[PlayerAnimation] PlayHitReaction triggered on {gameObject.name}", this);
        }

        /// <summary>
        /// Plays the death animation.
        /// </summary>
        public void PlayDeath()
        {
            animator.SetTrigger(DeathParam);
            Debug.Log($"[PlayerAnimation] PlayDeath triggered on {gameObject.name}", this);
        }

        /// <summary>
        /// Resets all animation triggers to prevent conflicts.
        /// </summary>
        private void ResetAllTriggers()
        {
            animator.ResetTrigger(DrawMeleeParam);
            animator.ResetTrigger(SheathParam);
        }

        /// <summary>
        /// Resets a specific animation trigger.
        /// </summary>
        public void ResetTrigger(string triggerName)
        {
            animator.ResetTrigger(triggerName);
            Debug.Log($"[PlayerAnimation] ResetTrigger: {triggerName} on {gameObject.name}", this);
        }
    }
}
