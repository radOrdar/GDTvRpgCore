using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable {
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private WeaponSO defaultWeapon;

        private WeaponSO currentWeaponSO;
        private GameObject currentWeaponInstance;
        private Mover mover;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake() {
            mover = GetComponent<Mover>();
        }

        private void Start() {
            if(currentWeaponInstance == null) EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponSO weapon) {
            if (weapon && rightHandTransform) {
                Destroy(currentWeaponInstance);
                currentWeaponInstance = weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
                currentWeaponSO = weapon;
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

            bool InRange() => Vector3.SqrMagnitude(target.transform.position - transform.position) < currentWeaponSO.WeaponRange * currentWeaponSO.WeaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= currentWeaponSO.TimeBetweenAttacks) {
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
            if (target == null) return;
            
            if (currentWeaponSO.HasProjectile) {
                currentWeaponSO.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            } else {
                target.TakeDamage(currentWeaponSO.WeaponDamage);
            }
        }

        void Shoot() {
            Hit();
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

        public object CaptureState() {
            return currentWeaponSO.name;
        }

        public void RestoreState(object state) {
            EquipWeapon(Resources.Load<WeaponSO>((string)state));
        }
    }
}