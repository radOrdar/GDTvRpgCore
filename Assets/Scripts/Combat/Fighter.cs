using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private float weaponDamage = 5f;

        private Mover mover;

        private Transform target;
        private float timeSinceLastAttack;

        private void Start() {
            mover = GetComponent<Mover>();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) { return; }

            if (!InRange()) {
                mover.MoveTo(target.position);
            } else {
                mover.Cancel();
                AttackingBehaviour();
            }

            bool InRange() => Vector3.SqrMagnitude(target.position - transform.position) < weaponRange * weaponRange;
        }

        private void AttackingBehaviour() {
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                timeSinceLastAttack = 0;
                GetComponent<Animator>().SetTrigger("attack");
            }
        }
        
        //animation event
        void Hit() {
            if (target != null && target.TryGetComponent(out Health health)) {
                health.TakeDamage(weaponDamage);
            }
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}