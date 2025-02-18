// using UnityEngine;

// namespace Player.Components
// {
//     public class PlayerWeapon : MonoBehaviour
//     {
//         [Header("Weapon Settings")]
//         public Weapon meleeWeapon;
//         public Weapon rangedWeapon;

//         private WeaponBaseState currentWeaponState;
//         private PlayerBase playerBase;

//         private void Awake()
//         {
//             playerBase = GetComponentInParent<PlayerBase>(); // Reference to the PlayerBase component
//         }

//         private void Start()
//         {
//             // Start with melee weapon state by default
//             EquipWeapon(meleeWeapon);
//         }

//         private void Update()
//         {
//             // Handle attack logic through the current weapon state
//             currentWeaponState?.HandleAttack();

//             // Switch between melee and ranged weapons based on player input
//             if (Input.GetKeyDown(KeyCode.Alpha1))  // Press 1 to equip melee weapon
//             {
//                 EquipWeapon(meleeWeapon);
//             }

//             if (Input.GetKeyDown(KeyCode.Alpha2))  // Press 2 to equip ranged weapon
//             {
//                 EquipWeapon(rangedWeapon);
//             }
//         }

//         // Equip the selected weapon and set the corresponding state
//         public void EquipWeapon(Weapon weapon)
//         {
//             if (weapon is MeleeWeapon)
//             {
//                 currentWeaponState = new MeleeWeaponState(playerBase, this);
//             }
//             else if (weapon is RangedWeapon)
//             {
//                 currentWeaponState = new RangedWeaponState(playerBase, this);
//             }
//         }
//     }
// }
