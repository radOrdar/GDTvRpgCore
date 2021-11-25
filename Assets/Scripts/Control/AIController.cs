using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float suspicionDuration = 1f;
        [SerializeField] private PatrolPath patrolPath;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Vector3 guardPosition;

        private float timeSinceLastSeen = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        
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
                timeSinceLastSeen = 0;
                if (fighter.CanAttack(player)) {
                    fighter.Attack(player);
                }
            } else if (timeSinceLastSeen < suspicionDuration) {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            } else {
                PatrolBehaviour();
            }

            timeSinceLastSeen += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWayPoint()) {
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            GetComponent<Mover>().StartMoveAction(nextPosition);
        }
        
        private bool AtWayPoint() {
            return Vector3.Distance(transform.position, GetCurrentWayPoint()) < 1f;
        }

        private void CycleWayPoint() {
            currentWaypointIndex++;
        }

        private Vector3 GetCurrentWayPoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }



        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}
