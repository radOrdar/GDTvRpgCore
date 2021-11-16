using System;
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
            bool inRange = Vector3.SqrMagnitude(target.position - transform.position) < weaponRange * weaponRange;
            if (target != null && !inRange) {
                mover.MoveTo(target.position);
            } else {
                mover.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget) {
            target = combatTarget.transform;
        }
    }
}