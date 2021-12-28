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
        [SerializeField] private WeaponSO defaultWeapon;

        private LazyValue<WeaponSO> currentWeapon;
        private GameObject currentWeaponModel;
        private Mover mover;

        public Health Target { get; private set; }
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake() {
            mover = GetComponent<Mover>();
            currentWeapon = new LazyValue<WeaponSO>(() => AttachWeapon(defaultWeapon));
        }

        private void Start() {
           currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponSO weapon) {
            currentWeapon.value = AttachWeapon(weapon);
        }

        private WeaponSO AttachWeapon(WeaponSO weapon) {
            Destroy(currentWeaponModel);
            currentWeaponModel = weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
            return weapon;
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

            bool InRange() => Vector3.SqrMagnitude(Target.transform.position - transform.position) < currentWeapon.value.WeaponRange * currentWeapon.value.WeaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= currentWeapon.value.TimeBetweenAttacks) {
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
            if (currentWeapon.value.HasProjectile) {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, Target, gameObject, damage);
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
            return currentWeapon.value.name;
        }

        public void RestoreState(object state) {
            EquipWeapon(Resources.Load<WeaponSO>((string)state));
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeapon.value.PercentageBonus;
            }
        }
    }
}