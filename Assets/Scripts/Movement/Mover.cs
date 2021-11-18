using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            animator.SetFloat("forwardSpeed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
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

    }
}