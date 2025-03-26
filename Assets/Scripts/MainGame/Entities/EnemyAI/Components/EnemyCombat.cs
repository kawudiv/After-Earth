using EnemyAI.Base;
using UnityEngine;
using Weapons.Base;
using Weapons.Types;

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

        public void EnableWeaponCollider()
        {
            if (weapon is MeleeWeapon meleeWeapon)
            {
                meleeWeapon.EnableWeaponCollider();
            }
            else
            {
                Debug.LogWarning(
                    $"[EnemyCombat] {gameObject.name} does not have a melee weapon with a collider!"
                );
            }
        }

        // Called from Animation Event (End of attack)
        public void DisableWeaponCollider()
        {
            if (weapon is MeleeWeapon meleeWeapon)
            {
                meleeWeapon.DisableWeaponCollider();
            }
            else
            {
                Debug.LogWarning(
                    $"[EnemyCombat] {gameObject.name} does not have a melee weapon with a collider!"
                );
            }
        }
    }
}
