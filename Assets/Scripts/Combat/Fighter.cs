using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        
        [SerializeField] private Transform handTransform;
        [SerializeField] private WeaponSO defaultWeapon;

        private WeaponSO currentWeapon;
        private Mover mover;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            EquipWeapon(defaultWeapon);
            mover = GetComponent<Mover>();
        }

        public void EquipWeapon(WeaponSO weapon) {
            if (weapon && handTransform) {
                weapon.Spawn(handTransform, GetComponent<Animator>());
                currentWeapon = weapon;
            }
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) { return; }

            if (target.IsDead) {
                Cancel();
                return;
            }

            if (!InRange()) {
                mover.MoveTo(target.transform.position);
            } else {
                mover.Cancel();
                AttackingBehaviour();
            }

            bool InRange() => Vector3.SqrMagnitude(target.transform.position - transform.position) < currentWeapon.WeaponRange * currentWeapon.WeaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= currentWeapon.TimeBetweenAttacks) {
                timeSinceLastAttack = 0;
                transform.LookAt(target.transform);
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
            if (target != null) {
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            target = null;
            mover.Cancel();
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}