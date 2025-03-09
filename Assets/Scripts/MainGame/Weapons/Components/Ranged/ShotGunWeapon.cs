// using UnityEngine;
// using Weapons.Types; // Ensure this namespace matches your project structure

// namespace Components.Ranged
// {
//     public class ShotGunWeapon : RangedWeapon
//     {
//         [Header("Shotgun Settings")]
//         public int pelletCount = 8;         // Number of pellets per shot
//         public float spreadAngle = 15f;     // Maximum spread angle (in degrees) for each pellet

//         private void Awake()
//         {
//             // Set default values for the shotgun.
//             weaponName = "Shotgun";
//             damage = 8f;              // Damage per pellet
//             attackSpeed = 1f;         // Time between shots
//             attackRange = 30f;        // Effective range of the shotgun

//             bulletSpeed = 25f;        // Speed at which pellets travel
//             bulletLifeTime = 2f;      // Lifetime of each pellet before being destroyed

//             // Ensure the attackPoint is set in the Inspector; otherwise, default to this transform.
//             if (attackPoint == null)
//             {
//                 attackPoint = transform;
//             }
//         }

//         public override void Attack()
//         {
//             // Ensure we have a bulletPrefab and an attackPoint.
//             if (bulletPrefab == null || attackPoint == null)
//             {
//                 Debug.LogWarning($"{weaponName} cannot attack because bulletPrefab or attackPoint is not assigned.");
//                 return;
//             }

//             // Fire multiple pellets.
//             for (int i = 0; i < pelletCount; i++)
//             {
//                 // Create a random spread rotation for each pellet.
//                 Quaternion randomSpread = Quaternion.Euler(
//                     Random.Range(-spreadAngle, spreadAngle),
//                     Random.Range(-spreadAngle, spreadAngle),
//                     0f
//                 );

//                 // Combine the attackPoint's rotation with the random spread.
//                 Quaternion pelletRotation = attackPoint.rotation * randomSpread;

//                 // Instantiate the pellet at the attack point with the computed rotation.
//                 GameObject pellet = Instantiate(bulletPrefab, attackPoint.position, pelletRotation);

//                 // Set the pellet's velocity if it has a Rigidbody.
//                 Rigidbody rb = pellet.GetComponent<Rigidbody>();
//                 if (rb != null)
//                 {
//                     rb.linearVelocity = pelletRotation * Vector3.forward * bulletSpeed;
//                 }
//                 else
//                 {
//                     Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
//                 }

//                 // Destroy the pellet after bulletLifeTime seconds to clean up.
//                 Destroy(pellet, bulletLifeTime);
//             }

//             Debug.Log($"{weaponName} fired with {pelletCount} pellets.");
//         }
//     }
// }
