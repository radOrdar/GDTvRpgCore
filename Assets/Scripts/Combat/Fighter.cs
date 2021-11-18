using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
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
                mover.Stop();   
            }
            
            bool InRange() => Vector3.SqrMagnitude(target.position - transform.position) < weaponRange * weaponRange;
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