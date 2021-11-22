
using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        
        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (health.IsDead) {
                enabled = false;
                return;
            }
            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange) {
                if (fighter.CanAttack(player)) {
                    fighter.Attack(player);
                }
            } else {
                fighter.Cancel();
            }
        }
    }
}
