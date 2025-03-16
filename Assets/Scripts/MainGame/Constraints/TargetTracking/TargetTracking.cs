using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Weapons.Types;

namespace Constraints.TargetTracking
{
    public class TargetTrackingController : MonoBehaviour
    {
        [Header("Rig Layers")]
        public Rig targetTracking; // ✅ Main Rig

        [Header("Constraints")]
        public MultiAimConstraint bodyTrackingConstraint;
        public MultiAimConstraint gunTrackingConstraint;
        public TwoBoneIKConstraint leftHandIKConstraint;
        public TwoBoneIKConstraint rightHandIKConstraint; // ✅ NEW: Right Hand IK

        [Header("Left Hand IK")]
        public Transform leftHandIKTarget; // ✅ Target inside Gun
        public Transform leftHandIK_Hint; // ✅ NEW: Left Hand Hint

        [Header("Right Hand IK")] // ✅ NEW: Right Hand IK Variables
        public Transform rightHandIKTarget; // ✅ Target inside RightHand
        public Transform rightHandIK_Hint; // ✅ NEW: Right Hand Hint

        private float defaultGunTrackingWeight = 0f;
        private float defaultBodyTrackingWeight = 0f;
        private float defaultLeftHandIKWeight = 0f;
        private float defaultRightHandIKWeight = 0f; // ✅ NEW: Right Hand Default Weight

        private Coroutine rigWeightRoutine;
        private Coroutine leftHandIKRoutine;
        private Coroutine rightHandIKRoutine; // ✅ NEW: Right Hand Coroutine

        private void Start()
        {
            Debug.Log("[TargetTrackingController] Start - Applying default settings.");
            targetTracking.weight = 0f; // Default: Off
            leftHandIKConstraint.weight = 0f; // Default: No IK
            rightHandIKConstraint.weight = 0f; // ✅ Default: No Right Hand IK
        }

        public void InitializeWeaponIK(RangedWeapon weapon)
        {
            if (weapon == null)
            {
                Debug.LogWarning("[TargetTrackingController] No weapon assigned!");
                return;
            }

            Debug.Log($"[TargetTrackingController] Initializing IK for {weapon.WeaponName}");

            // Save weapon-specific tracking weights
            defaultGunTrackingWeight = weapon.gunTrackingWeight;
            defaultBodyTrackingWeight = weapon.bodyTrackingWeight;
            defaultLeftHandIKWeight = weapon.leftHandIKWeight;
            defaultRightHandIKWeight = weapon.rightHandIKWeight; // ✅ NEW: Right Hand Weight

            // Update Left Hand IK Target
            if (leftHandIKTarget != null)
            {
                leftHandIKTarget.localPosition = weapon.leftHandIKPosition;
                leftHandIKTarget.localRotation = weapon.leftHandIKRotation;
                Debug.Log(
                    $"[TargetTrackingController] LeftHandIK Updated: Pos={weapon.leftHandIKPosition}, Rot={weapon.leftHandIKRotation.eulerAngles}"
                );
            }
            else
            {
                Debug.LogError("[TargetTrackingController] LeftHandIK Target is NULL!");
            }

            // ✅ NEW: Update Left Hand IK Hint
            if (leftHandIK_Hint != null)
            {
                leftHandIK_Hint.localPosition = weapon.leftHandIKHintPosition;
                Debug.Log(
                    $"[TargetTrackingController] LeftHandIK_Hint Updated: Pos={weapon.leftHandIKHintPosition}"
                );
            }
            else
            {
                Debug.LogError("[TargetTrackingController] LeftHandIK_Hint is NULL!");
            }

            // ✅ NEW: Update Right Hand IK Target
            if (rightHandIKTarget != null)
            {
                rightHandIKTarget.localPosition = weapon.rightHandIKPosition;
                rightHandIKTarget.localRotation = weapon.rightHandIKRotation;
                Debug.Log(
                    $"[TargetTrackingController] RightHandIK Updated: Pos={weapon.rightHandIKPosition}, Rot={weapon.rightHandIKRotation.eulerAngles}"
                );
            }
            else
            {
                Debug.LogError("[TargetTrackingController] RightHandIK Target is NULL!");
            }

            // ✅ NEW: Update Right Hand IK Hint
            if (rightHandIK_Hint != null)
            {
                rightHandIK_Hint.localPosition = weapon.rightHandIKHintPosition;
                Debug.Log(
                    $"[TargetTrackingController] RightHandIK_Hint Updated: Pos={weapon.rightHandIKHintPosition}"
                );
            }
            else
            {
                Debug.LogError("[TargetTrackingController] RightHandIK_Hint is NULL!");
            }
        }

        public void SetAiming(bool isAiming)
        {
            Debug.Log($"[TargetTrackingController] Aiming: {isAiming}");

            float transitionTime = 0.3f;

            if (rigWeightRoutine != null)
                StopCoroutine(rigWeightRoutine);
            rigWeightRoutine = StartCoroutine(
                AdjustRigWeight(targetTracking, isAiming ? 1f : 0f, transitionTime)
            );

            if (leftHandIKRoutine != null)
                StopCoroutine(leftHandIKRoutine);
            leftHandIKRoutine = StartCoroutine(
                AdjustRigWeight(
                    leftHandIKConstraint,
                    isAiming ? defaultLeftHandIKWeight : 0f,
                    transitionTime
                )
            );

            if (rightHandIKRoutine != null)
                StopCoroutine(rightHandIKRoutine);
            rightHandIKRoutine = StartCoroutine(
                AdjustRigWeight(
                    rightHandIKConstraint,
                    isAiming ? defaultRightHandIKWeight : 0f,
                    transitionTime
                )
            );

            if (isAiming)
            {
                SetConstraintWeight(bodyTrackingConstraint, defaultBodyTrackingWeight);
                SetConstraintWeight(gunTrackingConstraint, defaultGunTrackingWeight);
            }
            else
            {
                SetConstraintWeight(bodyTrackingConstraint, 0f);
                SetConstraintWeight(gunTrackingConstraint, 0f);
            }
        }

        /// <summary>
        /// Smoothly adjusts the Rig weight over time.
        /// </summary>
        private IEnumerator AdjustRigWeight(Rig rig, float targetWeight, float duration)
        {
            float startWeight = rig.weight;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                rig.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
                yield return null;
            }

            rig.weight = targetWeight;
            Debug.Log($"[TargetTrackingController] {rig.name} Final Weight: {rig.weight}");
        }

        /// <summary>
        /// Smoothly adjusts the Constraint weight over time.
        /// </summary>
        private IEnumerator AdjustRigWeight(
            TwoBoneIKConstraint constraint,
            float targetWeight,
            float duration
        )
        {
            float startWeight = constraint.weight;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                constraint.weight = Mathf.Lerp(startWeight, targetWeight, elapsed / duration);
                yield return null;
            }

            constraint.weight = targetWeight;
            Debug.Log(
                $"[TargetTrackingController] {constraint.name} Final Weight: {constraint.weight}"
            );
        }

        private void SetConstraintWeight(MultiAimConstraint constraint, float weight)
        {
            if (constraint != null)
            {
                constraint.weight = weight;
                Debug.Log($"[TargetTrackingController] {constraint.name} Weight Set: {weight}");
            }
        }
    }
}
