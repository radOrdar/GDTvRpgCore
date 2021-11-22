using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Vector3 guardPosition;
        
        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
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
                GetComponent<Mover>().StartMoveAction(guardPosition);
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
