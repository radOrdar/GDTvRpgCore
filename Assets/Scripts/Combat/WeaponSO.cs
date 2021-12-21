using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponSO : ScriptableObject {
        [SerializeField] private GameObject equippedPrefab;
        [SerializeField] private AnimatorOverrideController weaponOverride;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 5f;
        public float WeaponDamage => weaponDamage;
        public float TimeBetweenAttacks => timeBetweenAttacks; 
        public float WeaponRange => weaponRange;

        public void Spawn(Transform handTransform, Animator animator) {
            if(equippedPrefab) Instantiate(equippedPrefab, handTransform);
            if(weaponOverride) animator.runtimeAnimatorController = weaponOverride;
        }
    }
}