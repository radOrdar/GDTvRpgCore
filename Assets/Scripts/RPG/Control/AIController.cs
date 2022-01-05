using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float suspicionDuration = 1f;
        [SerializeField] private float aggroDuration = 3f;
        [SerializeField] private float dwellDuration = 1f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float patrolSpeed = 3;
        [SerializeField] private float chaseSpeed = 5;
        [SerializeField] private float aggrevateNearbyEnemiesRadius = 5;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;

        private LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedToWaypoint = 0;
        private float timeSinceAggrevated = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        private void Awake() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(() => transform.position);
        }

        //transfrom is a gameobject and may not be initialized when called in Awake() ??? 
        private void Start() {
            guardPosition.ForceInit();
        }

        //what about state machine? ?? ? 
        private void Update() {
            if (health.IsDead) {
                enabled = false;
                return;
            }

            if (IsAggrevated() && fighter.CanAttack(player)) {
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspicionDuration) {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            } else {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0;
            mover.Speed = chaseSpeed;
            fighter.Attack(player);
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies() {
            //in update() (-_-)
            var colliders = Physics.OverlapSphere(transform.position, aggrevateNearbyEnemiesRadius);
            foreach (var collider in colliders) {
                if (collider.TryGetComponent(out AIController enemy)) {
                    enemy.Aggrevate();
                }
            }
        }

        public void Aggrevate() {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedToWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private bool IsAggrevated() {
            return Vector3.Distance(transform.position, player.transform.position) < chaseRange || timeSinceAggrevated < aggroDuration;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition.value;
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