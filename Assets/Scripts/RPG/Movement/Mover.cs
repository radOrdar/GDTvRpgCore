using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction, ISaveable {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;

        public float Speed {
            get => navMeshAgent.speed;
            set => navMeshAgent.speed = value;
        }

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            if (health.IsDead) {
                navMeshAgent.enabled = false;
                enabled = false;
                return;
            }
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            animator.SetFloat("forwardSpeed", navMeshAgent.velocity.magnitude);
        }

        public void StartMoveAction(Vector3 destination) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 point) {
            navMeshAgent.SetDestination(point);
            navMeshAgent.isStopped = false;
        }

        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}