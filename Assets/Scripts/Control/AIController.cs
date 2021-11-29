using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float suspicionDuration = 1f;
        [SerializeField] private float dwellDuration = 1f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float patrolSpeed = 3;
        [SerializeField] private float chaseSpeed = 5;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;

        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedToWaypoint = 0;
        private int currentWaypointIndex = 0;

        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }

        private void Update() {
            if (health.IsDead) {
                enabled = false;
                return;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange) {
                timeSinceLastSawPlayer = 0;
                if (fighter.CanAttack(player)) {
                    mover.Speed = chaseSpeed;
                    fighter.Attack(player);
                }
            } else if (timeSinceLastSawPlayer < suspicionDuration) {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            } else {
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedToWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;
            mover.Speed = patrolSpeed;
            
            if (patrolPath != null) {
                if (AtWayPoint()) {
                    timeSinceArrivedToWaypoint = 0f;
                    CycleWayPoint();
                }

                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArrivedToWaypoint > dwellDuration) {
                mover = GetComponent<Mover>();
                mover.StartMoveAction(nextPosition);
            }
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