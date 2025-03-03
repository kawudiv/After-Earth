using Player.Base;
using UnityEngine;
using Weapons.Base;
using Weapons.Types;

namespace Player.Components
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponBase equippedMeleeWeapon;
        private WeaponBase equippedRangedWeapon;

        public WeaponBase EquippedMeleeWeapon => equippedMeleeWeapon;
        public WeaponBase EquippedRangedWeapon => equippedRangedWeapon;

        [SerializeField]
        private Transform weaponHolder;

        [SerializeField]
        private Transform dropPoint;
        private PlayerAnimation playerAnimation;
        private PlayerBase player;

        private void Awake()
        {
            player = GetComponent<PlayerBase>();
            playerAnimation = GetComponent<PlayerAnimation>();

            if (playerAnimation == null)
            {
                Debug.LogError("[PlayerInventory] PlayerAnimation component is missing!");
            }
        }

        /// <summary>
        /// Returns the currently equipped weapon (melee or ranged).
        /// </summary>
        /// <returns>The currently equipped weapon, or null if no weapon is equipped.</returns>
        public WeaponBase GetEquippedWeapon()
        {
            if (equippedMeleeWeapon != null)
            {
                return equippedMeleeWeapon;
            }
            else if (equippedRangedWeapon != null)
            {
                return equippedRangedWeapon;
            }
            return null;
        }

        /// <summary>
        /// Returns the type of the currently equipped weapon.
        /// </summary>
        /// <returns>"Melee", "Ranged", or "None" if no weapon is equipped.</returns>
        public string GetEquippedWeaponType()
        {
            if (equippedMeleeWeapon != null)
            {
                return "Melee";
            }
            else if (equippedRangedWeapon != null)
            {
                return "Ranged";
            }
            return "None";
        }

        public void TryPickUpWeapon()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliders)
            {
                ItemPickup pickup = collider.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    WeaponBase weapon = pickup.weaponPrefab;

                    FindAnyObjectByType<PlayerSlotInventory>()?.AddWeapon(weapon);


                    // Hide the pickup item
                    pickup.gameObject.SetActive(false);
                    return;
                }
            }
            Debug.Log("[PlayerInventory] No weapon found to pick up.");
        }


        public void EquipWeapon(WeaponBase newWeapon)
        {
            if (newWeapon == null)
            {
                Debug.LogError("[PlayerInventory] Trying to equip a NULL weapon!");
                return;
            }

            Debug.Log(
                $"[PlayerInventory] Attempting to equip {newWeapon.weaponName} ({newWeapon.GetType().Name})"
            );

            if (newWeapon is MeleeWeapon melee)
            {
                if (equippedMeleeWeapon != null)
                {
                    Debug.Log(
                        "[PlayerInventory] Dropping old melee weapon before equipping new one."
                    );
                    DropWeapon(equippedMeleeWeapon);
                }

                equippedMeleeWeapon = melee;
                equippedMeleeWeapon.transform.SetParent(weaponHolder); // Attach to player

                // ✅ Apply Position & Rotation Offsets
                equippedMeleeWeapon.ApplyEquipTransform(equippedMeleeWeapon.transform);


                if (playerAnimation != null)
                {
                    playerAnimation.SetMeleeWeaponType(melee.weaponTypeID);
                    playerAnimation.SetTrigger("DrawMelee");
                    WeaponDrawnToggle(true);
                }

                Debug.Log(
                    $"✅ [PlayerInventory] Melee Weapon Equipped: {equippedMeleeWeapon.weaponName}"
                );
            }
            else if (newWeapon is RangedWeapon ranged)
            {
                if (equippedRangedWeapon != null)
                {
                    Debug.Log(
                        "[PlayerInventory] Dropping old ranged weapon before equipping new one."
                    );
                    DropWeapon(equippedRangedWeapon);
                }

                equippedRangedWeapon = ranged;
                equippedRangedWeapon.transform.SetParent(weaponHolder); // Attach to player

                // ✅ Apply Position & Rotation Offsets
                equippedRangedWeapon.ApplyEquipTransform(equippedRangedWeapon.transform);

                // ✅ Trigger the draw animation (if applicable)
                GetComponent<PlayerAnimation>()
                    ?.SetTrigger("DrawRanged");

                Debug.Log(
                    $"✅ [PlayerInventory] Ranged Weapon Equipped: {equippedRangedWeapon.weaponName}"
                );
            }
        }

        public void DropCurrentWeapon()
        {
            if (equippedMeleeWeapon == null && equippedRangedWeapon == null)
            {
                Debug.Log("[PlayerInventory] No weapon equipped to drop.");
                return;
            }

            if (equippedMeleeWeapon != null)
            {
                DropWeapon(equippedMeleeWeapon);
                equippedMeleeWeapon = null;
            }
            else if (equippedRangedWeapon != null)
            {
                DropWeapon(equippedRangedWeapon);
                equippedRangedWeapon = null;
            }
        }

        public void DropWeapon(WeaponBase weaponToDrop)
        {
            if (weaponToDrop == null)
                return;

            // ✅ Remove it from the player's hand
            weaponToDrop.transform.SetParent(null);

            // ✅ Drop in front of the player
            weaponToDrop.transform.position = transform.position + transform.forward * 1f;
            weaponToDrop.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            weaponToDrop.gameObject.SetActive(true);

            Debug.Log($"✅ [PlayerInventory] Dropped {weaponToDrop.weaponName}");

            // ✅ Trigger the "DropWeapon" animation instantly
            if (playerAnimation != null)
            {
                playerAnimation.SetTrigger("DropWeapon");
                WeaponDrawnToggle(false);
            }

            // ✅ Clear the equipped weapon reference
            if (weaponToDrop is MeleeWeapon)
            {
                equippedMeleeWeapon = null;
            }
            else
            {
                equippedRangedWeapon = null;
            }
        }
        public void WeaponDrawnToggle(bool value)
        {
            player.IsWeaponDrawn = value;
        }


        //ForDebuggingPurposesOnly

        public void DebugEquippedWeapon()
        {
            if (weaponHolder.childCount > 0)
            {
                Debug.Log("🗑️[PlayerInventory] Weapon in Slot: " + weaponHolder.GetChild(0).name);
            }
            else
            {
                Debug.Log("🗑️[PlayerInventory] No Weapon equipped");
            }
        }

    void Update()
    {
        Debug.Log("[PlayerInventory] Update() is running"); // ✅ Check if Update() is working

        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugEquippedWeapon();
        }
    }



    }
}
