using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfigSO : ScriptableObject {
        [SerializeField] private Weapon equippedPrefab;
        [SerializeField] private AnimatorOverrideController weaponOverride;
        [SerializeField] private bool isRightHanded = true;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageBonus = 0;
        [SerializeField] private Projectile projectilePrefab = null;
        public float WeaponDamage => weaponDamage;
        public float TimeBetweenAttacks => timeBetweenAttacks;
        public float WeaponRange => weaponRange;
        public bool HasProjectile => projectilePrefab != null;
        public float PercentageBonus => percentageBonus;


        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator) {
            if (weaponOverride) {
                animator.runtimeAnimatorController = weaponOverride;
            } else {
                var animatorOverride = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (animatorOverride) {
                    animator.runtimeAnimatorController = animatorOverride.runtimeAnimatorController;
                }
            }

            Weapon equippedWeapon = null;
            if (equippedPrefab) equippedWeapon = Instantiate(equippedPrefab, isRightHanded ? rightHandTransform : leftHandTransform);
            return equippedWeapon;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) {
            if (!projectilePrefab) return;
            Projectile projectile = Instantiate(projectilePrefab,
                isRightHanded ? rightHand.position : leftHand.position,
                Quaternion.identity);
            projectile.SetTarget(target, calculatedDamage, instigator);
        }
    }
}