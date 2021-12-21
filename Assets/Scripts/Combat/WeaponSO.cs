﻿using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponSO : ScriptableObject {
        [SerializeField] private GameObject equippedPrefab;
        [SerializeField] private AnimatorOverrideController weaponOverride;
        [SerializeField] private bool isRightHanded = true;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private Projectile projectilePrefab = null;
        public float WeaponDamage => weaponDamage;
        public float TimeBetweenAttacks => timeBetweenAttacks;
        public float WeaponRange => weaponRange;
        public bool HasProjectile => projectilePrefab != null;

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator) {
            if (equippedPrefab)
                Instantiate(equippedPrefab,
                    isRightHanded ? rightHandTransform : leftHandTransform);
            if (weaponOverride) animator.runtimeAnimatorController = weaponOverride;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
            if (!projectilePrefab) return;
            Projectile projectile = Instantiate(projectilePrefab,
                isRightHanded ? rightHand.position : leftHand.position,
                Quaternion.identity);
            projectile.SetTarget(target, weaponDamage);
        }
    }
}