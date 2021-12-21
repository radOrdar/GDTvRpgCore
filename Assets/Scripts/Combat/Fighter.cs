using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private Transform handTransform;
        [SerializeField] private AnimatorOverrideController weaponOverride;

        private Mover mover;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            SpawnWeapon();
            mover = GetComponent<Mover>();
        }

        private void SpawnWeapon() {
            if (weaponPrefab && handTransform) {
                Instantiate(weaponPrefab, handTransform);
            }

            if (weaponOverride) {
                GetComponent<Animator>().runtimeAnimatorController = weaponOverride;
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

            bool InRange() => Vector3.SqrMagnitude(target.transform.position - transform.position) < weaponRange * weaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= timeBetweenAttacks) {
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
                target.TakeDamage(weaponDamage);
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