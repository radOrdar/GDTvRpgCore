using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;

        private Transform target;
        private Mover mover;

        private void Start() {
            mover = GetComponent<Mover>();
        }

        private void Update() {
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
            GetComponent<Animator>().SetTrigger("attack");
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
        
        void Hit() {}
    }
}