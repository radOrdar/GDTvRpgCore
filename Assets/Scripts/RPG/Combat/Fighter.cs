using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider {
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private WeaponConfigSO defaultWeaponConfig;

        private LazyValue<WeaponConfigSO> currentWeaponConfig;
        private Weapon equippedWeapon;
        private Mover mover;

        public Health Target { get; private set; }
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake() {
            mover = GetComponent<Mover>();
            currentWeaponConfig = new LazyValue<WeaponConfigSO>(() => AttachWeapon(defaultWeaponConfig));
        }

        private void Start() {
           currentWeaponConfig.ForceInit();
        }

        public void EquipWeapon(WeaponConfigSO weaponConfig) {
            currentWeaponConfig.value = AttachWeapon(weaponConfig);
        }

        private WeaponConfigSO AttachWeapon(WeaponConfigSO weaponConfig) {
            if(equippedWeapon) Destroy(equippedWeapon.gameObject);
            if(weaponConfig){ equippedWeapon = weaponConfig.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());}
            return weaponConfig;
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (Target == null) { return; }

            if (Target.IsDead) {
                Cancel();
                return;
            }

            if (!InRange()) {
                mover.MoveTo(Target.transform.position);
            } else {
                mover.Cancel();
                AttackingBehaviour();
            }

            bool InRange() => Vector3.SqrMagnitude(Target.transform.position - transform.position) < currentWeaponConfig.value.WeaponRange * currentWeaponConfig.value.WeaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= currentWeaponConfig.value.TimeBetweenAttacks) {
                timeSinceLastAttack = 0;
                transform.LookAt(Target.transform);
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        //animation event
        void Hit() {
            if (Target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (equippedWeapon) equippedWeapon.OnHit();
            if (currentWeaponConfig.value.HasProjectile) {
                currentWeaponConfig.value.LaunchProjectile(rightHandTransform, leftHandTransform, Target, gameObject, damage);
            } else {
                Target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot() {
            Hit();
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            Target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            Target = null;
            mover.Cancel();
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState() {
            return currentWeaponConfig.value.name;
        }

        public void RestoreState(object state) {
            EquipWeapon(Resources.Load<WeaponConfigSO>((string)state));
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaponConfig.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaponConfig.value.PercentageBonus;
            }
        }
    }
}