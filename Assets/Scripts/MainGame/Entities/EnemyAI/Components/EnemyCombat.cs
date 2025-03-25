using EnemyAI.Base;
using UnityEngine;
using Weapons.Base;

namespace EnemyAI.Components
{
    public class EnemyCombat : MonoBehaviour
    {
        private EnemyBase enemy;
        private WeaponBase weapon;
        private EnemyAnimation enemyAnimation;
        private EnemySound enemySound;

        private void Awake()
        {
            enemy = GetComponent<EnemyBase>();
            enemyAnimation = GetComponent<EnemyAnimation>();
            enemySound = GetComponent<EnemySound>();

            // Get the weapon from the first child
            weapon = GetComponentInChildren<WeaponBase>();
            if (weapon == null)
            {
                Debug.LogError($"[EnemyCombat] No weapon found in children of {gameObject.name}!");
            }
            else
            {
                Debug.Log($"[EnemyCombat] Found weapon: {weapon.name}");
            }
        }

        public void PerformAttack()
        {
            if (weapon == null)
            {
                Debug.LogWarning($"[EnemyCombat] No weapon equipped on {gameObject.name}!");
                return;
            }

            Debug.Log($"[EnemyCombat] {gameObject.name} performing attack");
            enemyAnimation.SetTrigger("Attack");
            weapon.Attack();
        }

        // Called from Animation Event (Start of attack)
        // public void EnableWeaponCollider()
        // {
        //     weapon?.EnableWeaponCollider();
        // }

        // // Called from Animation Event (End of attack)
        // public void DisableWeaponCollider()
        // {
        //     weapon?.DisableWeaponCollider();
        // }
    }
}
