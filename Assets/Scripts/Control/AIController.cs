
using System;
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;

        private Fighter fighter;
        private GameObject player; 
        
        private void Start() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
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
